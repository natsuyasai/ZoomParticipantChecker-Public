using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZoomParticipantChecker.Data;

namespace ZoomParticipantChecker.Model
{
    internal interface IMonitoringFacade
    {
        IEnumerable<MonitoringInfo> GetMonitoringInfos();
        bool IsAllJoin();
        bool IsEnableAutoScroll();
        void Pause();
        void RegisterMonitoringTargets(IEnumerable<string> targetUsers);
        void Resume();
        Task<bool> SelectZoomParticipantElement(Action onDetectedTargetElemetCallback);
        void SetEnableAutoScroll(bool val);
        void SetParticipantAuto(int target);
        Task StartMonitoring(Action onJoinStateChangeCallback);
        void StopMonitoring();
        void SwitchingParticipantState(int target);
    }
}