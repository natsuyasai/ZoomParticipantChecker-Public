using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UIAutomationClient;
using ZoomParticipantChecker.Data;
using ZoomParticipantChecker.Model.AutomationElementUtil;
using ZoomParticipantChecker.Util;

namespace ZoomParticipantChecker.Model
{
    /// <summary>
    /// 監視関連ファサードクラス
    /// </summary>
    internal class MonitoringFacade : IMonitoringFacade
    {
        /// <summary>
        /// 参加者リスト取得
        /// </summary>
        private readonly AutomationElementGetter _automationElementGetter;

        /// <summary>
        /// 監視
        /// </summary>
        private readonly MonitoringModel _monitoringModel;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MonitoringFacade()
        {
            _automationElementGetter = App.Services.GetService<AutomationElementGetter>();
            var targetName = AppSettingsManager.ParticipantListName;
            if (!string.IsNullOrEmpty(targetName))
            {
                _automationElementGetter = new AutomationElementGetter(targetName);
            }
            _monitoringModel = App.Services.GetService<MonitoringModel>();
        }


        /// <summary>
        /// Zoom参加者要素選択
        /// </summary>
        /// <returns></returns>
        public async Task<bool> SelectZoomParticipantElement(Action onDetectedTargetElemetCallback)
        {
            await Task.Run(() =>
            {
                _automationElementGetter.SubscribeToFocusChange(() =>
                {
                    Console.WriteLine("対象エレメント検知");
                    onDetectedTargetElemetCallback();
                });
            });
            return false;
        }

        /// <summary>
        /// 監視情報取得
        /// </summary>
        public IEnumerable<MonitoringInfo> GetMonitoringInfos()
        {
            return _monitoringModel.GetMonitoringInfos();
        }

        /// <summary>
        /// 自動スクロールが有効か
        /// </summary>
        /// <returns></returns>
        public bool IsEnableAutoScroll()
            => _monitoringModel.IsEnableAutoScroll;

        /// <summary>
        /// 自動スクロールが有効か
        /// </summary>
        /// <returns></returns>
        public void SetEnableAutoScroll(bool val)
        {
            _monitoringModel.IsEnableAutoScroll = val;
        }

        /// <summary>
        /// 監視対象登録
        /// </summary>
        /// <param name="targetUsers"></param>
        public void RegisterMonitoringTargets(IEnumerable<string> targetUsers)
        {
            _monitoringModel.RegisterMonitoringTargets(targetUsers);
        }

        /// <summary>
        /// 監視開始
        /// </summary>
        public async Task StartMonitoring(Action onJoinStateChangeCallback)
        {
            if (_automationElementGetter.TargetElement == null)
            {
                throw new ArgumentException("対象の要素が見つかっていません");
            }
            Console.WriteLine("タスク開始");
            var infoGetter = new AutomationElementChildNameInfoGetter(new CUIAutomation(), _automationElementGetter.TargetElement, null);
            await _monitoringModel.StartMonitoring(onJoinStateChangeCallback, infoGetter);
        }

        /// <summary>
        /// 監視一時停止
        /// </summary>
        public void Pause()
        {
            Console.WriteLine("タスク一時停止");
            _monitoringModel.Pause();
        }

        /// <summary>
        /// 監視再開
        /// </summary>
        public void Resume()
        {
            Console.WriteLine("タスク再開");
            _monitoringModel.Resume();
        }

        /// <summary>
        /// 監視停止
        /// </summary>
        public void StopMonitoring()
        {
            Console.WriteLine("タスク停止");
            _automationElementGetter.UnsubscribeFocusChange();
            _monitoringModel.StopMonitoring();
        }

        /// <summary>
        /// 参加状態切り替え
        /// </summary>
        /// <param name="target"></param>
        public void SwitchingParticipantState(int target)
        {
            _monitoringModel.SwitchingParticipantState(target);
        }

        /// <summary>
        /// 全参加
        /// </summary>
        /// <returns></returns>
        public bool IsAllJoin()
            => _monitoringModel.IsAllJoin();


        /// <summary>
        /// 監視状態を自動に設定
        /// </summary>
        /// <param name="target"></param>
        public void SetParticipantAuto(int target)
        {
            _monitoringModel.SetParticipantAuto(target);
        }
    }
}
