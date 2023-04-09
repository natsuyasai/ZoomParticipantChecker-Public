using System.Collections.Generic;
using System.Threading.Tasks;
using ZoomParticipantChecker.Data;

namespace ZoomParticipantChecker.Model
{
    internal interface IPresetModel
    {
        void Clear();
        IEnumerable<string> GetCurrentPresetDataList();
        string GetCurrntPresetFilePath();
        IEnumerable<PresetInfo> GetPreset();
        IEnumerable<string> GetPresetDataList(int index);
        IEnumerable<string> GetPresetNameList();
        Task<bool> ReadPresetData(string rootPath, string targetFolderName);
        void UpdateCurrentIndex(int id);
    }
}