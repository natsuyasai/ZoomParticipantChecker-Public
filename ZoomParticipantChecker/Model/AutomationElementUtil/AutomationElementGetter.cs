using System;
using UIAutomationClient;

namespace ZoomParticipantChecker.Model.AutomationElementUtil
{
    /// <summary>
    /// AutomationElement取得用クラス
    /// </summary>
    /// <remarks>
    /// https://docs.microsoft.com/ja-jp/dotnet/framework/ui-automation/subscribe-to-ui-automation-events
    /// https://docs.microsoft.com/ja-jp/windows/win32/winauto/uiauto-eventsforclients
    /// </remarks>
    internal class AutomationElementGetter
    {
        /// <summary>
        /// 対象の要素
        /// </summary>
        public IUIAutomationElement TargetElement { get; private set; } = null;

        /// <summary>
        /// フォーカスイベントハンドラ
        /// </summary>
        private FocusChangeHandler _focusHandler = null;


        /// <summary>
        /// 対象の要素検出コールバック
        /// </summary>
        private Action _onDetectedTargetElemetCallback = null;

        /// <summary>
        /// CUIAutomation
        /// </summary>
        private readonly CUIAutomation _automation;

        /// <summary>
        /// 取得対象名
        /// </summary>
        private readonly string _targetName;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="targetName"></param>
        public AutomationElementGetter(string targetName)
        {
            _targetName = targetName;
            _automation = new CUIAutomation();
        }

        /// <summary>
        /// フォーカスイベント購読
        /// </summary>
        public void SubscribeToFocusChange(Action onDetectedTargetElemetCallback)
        {
            TargetElement = null;
            _onDetectedTargetElemetCallback = onDetectedTargetElemetCallback;
            if (_focusHandler != null)
            {
                UnsubscribeFocusChange();
            }
            _focusHandler = new FocusChangeHandler(OnFocusChange);
            _automation.AddFocusChangedEventHandler(null, _focusHandler);
        }

        /// <summary>
        /// フォーカスイベント購読破棄
        /// </summary>
        public void UnsubscribeFocusChange()
        {
            if (_focusHandler != null)
            {
                _automation.RemoveFocusChangedEventHandler(_focusHandler);
                _focusHandler = null;
            }
        }

        /// <summary>
        /// フォーカスイベント
        /// </summary>
        private void OnFocusChange(IUIAutomationElement element)
        {
            if (TargetElement != null)
            {
                return;
            }
            try
            {
                if (element.CurrentName.Contains(_targetName))
                {
                    SetTargetElement(element);
                }
                else
                {
                    // 表示リストの要素が選択状態だと，表示リストへのフォーカスイベントが発生しないため，
                    // 順に親をたどって探す(参加者要素の親の親が参加者リストのため，2つ上までにする)
                    var ret = TryGetParentElement(element);
                    if (ret != null)
                    {
                        TryGetParentElement(ret);
                    }
                }
            }
            catch
            {
                Console.WriteLine("要素確認失敗");
            }
        }

        /// <summary>
        /// 親要素取得
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private IUIAutomationElement TryGetParentElement(IUIAutomationElement current)
        {
            var condition = _automation.CreatePropertyCondition(UIAutomationIdDefine.UIA_ControlTypePropertyId, UIAutomationIdDefine.UIA_ListControlTypeId);
            var walker = _automation.CreateTreeWalker(condition);
            var parent = walker.GetParentElement(current);
            if (parent != null)
            {
                if (parent.CurrentName.Contains(_targetName))
                {
                    SetTargetElement(parent);
                }
            }
            return parent;
        }

        /// <summary>
        /// 対象要素設定 
        /// </summary>
        /// <param name="element"></param>
        private void SetTargetElement(IUIAutomationElement element)
        {
            TargetElement = element;
            _onDetectedTargetElemetCallback?.Invoke();
            UnsubscribeFocusChange();
        }

        /// <summary>
        /// イベントハンドラ
        /// </summary>
        private class FocusChangeHandler : IUIAutomationFocusChangedEventHandler
        {
            private readonly Action<IUIAutomationElement> Handler;
            /// <summary>
            /// コンストラクタ
            /// </summary>
            public FocusChangeHandler(Action<IUIAutomationElement> handler)
            {
                Handler = handler;
            }
            /// <summary>
            /// イベント
            /// </summary>
            /// <param name="sender"></param>
            public void HandleFocusChangedEvent(IUIAutomationElement sender)
            {
                Handler(sender);
            }
        }
    }
}
