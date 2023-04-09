﻿using Microsoft.VisualBasic.FileIO;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoomParticipantChecker.Data;
using static ZoomParticipantChecker.Data.PresetInfo;

namespace ZoomParticipantChecker.Model
{
    internal class PresetModel : IPresetModel
    {
        /// <summary>
        /// プリセット
        /// </summary>
        private List<PresetInfo> _preset = new List<PresetInfo>();

        /// <summary>
        /// 現在のインデックス
        /// </summary>
        private int _currentIndex = 0;

        /// <summary>
        /// デフォルトエンコード
        /// </summary>
        private readonly Encoding DefEncoding = Encoding.GetEncoding("Shift_JIS");

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PresetModel()
        {
        }

        /// <summary>
        /// クリア
        /// </summary>
        public void Clear()
        {
            _preset.Clear();
        }

        /// <summary>
        /// プリセット情報取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PresetInfo> GetPreset()
        {
            return _preset;
        }

        /// <summary>
        /// プリセット名一覧取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPresetNameList()
        {
            return _preset.Select(info => info.Name);
        }

        /// <summary>
        /// プリセットデータ一覧取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetPresetDataList(int index)
        {
            if (index >= 0 && index < _preset.Count)
            {
                return _preset[index].Data;
            }
            else
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// 現在のプリセットデータ一覧取得
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetCurrentPresetDataList()
        {
            return GetPresetDataList(_currentIndex);
        }

        /// <summary>
        /// 現在のID更新
        /// </summary>
        /// <param name="id"></param>
        public void UpdateCurrentIndex(int id)
        {
            var index = _preset.FindIndex(info => info.IsMine(id));
            if (index >= 0)
            {
                _currentIndex = index;
            }
        }

        /// <summary>
        /// 現在のプリセットのファイルパス取得
        /// </summary>
        /// <returns></returns>
        public string GetCurrntPresetFilePath()
        {
            if (_currentIndex >= 0 && _currentIndex < _preset.Count)
            {
                return _preset[_currentIndex].FilePath;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// プリセット読み込み
        /// </summary>
        /// <param name="rootPath"></param>
        public Task<bool> ReadPresetData(string rootPath, string targetFolderName)
        {
            _preset.Clear();
            _currentIndex = 0;
            var targetFolder = rootPath + "\\" + targetFolderName;
            try
            {
                // 存在しない場合はテンプレートを生成
                CreateInitData(targetFolder);
                var id = 0;
                foreach (var file in Directory.EnumerateFiles(targetFolder))
                {
                    var data = new List<string>();
                    using (var parser = new TextFieldParser(Path.Combine(targetFolder, file), DefEncoding))
                    {
                        parser.Delimiters = new string[] { "," };
                        while (!parser.EndOfData)
                        {
                            data.Add(string.Join(",", parser.ReadFields()));
                        }
                    }
                    _preset.Add(new PresetInfo(id, file, Path.GetFileNameWithoutExtension(file), data));
                    id++;
                }
                _preset.Sort(new PresetInfoNaturalStringComparer());
                return Task.FromResult(true);
            }
            catch
            {
                _preset = new List<PresetInfo>() { new ErrorPresetInfo() };
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// 初期データ作成
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <returns></returns>
        private bool CreateInitData(string targetFolder)
        {
            if (Directory.Exists(targetFolder))
            {
                return CreateInitFile(targetFolder);
            }
            else
            {
                Directory.CreateDirectory(targetFolder);
                CreateInitFile(targetFolder);
                return true;
            }
        }

        /// <summary>
        /// 初期ファイル作成
        /// </summary>
        /// <param name="targetFolder"></param>
        private bool CreateInitFile(string targetFolder)
        {
            try
            {
                if (Directory.Exists(targetFolder))
                {
                    if (Directory.GetFiles(targetFolder, "*csv", System.IO.SearchOption.TopDirectoryOnly).Length == 0)
                    {
                        var path = Path.Combine(targetFolder, "テンプレートプリセット1.csv");
                        using (StreamWriter streamWriter = new StreamWriter(File.Open(path, FileMode.Create), DefEncoding))
                        {
                            streamWriter.WriteLine("テンプレート1");
                            streamWriter.WriteLine("テンプレート2");
                        }
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
