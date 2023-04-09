﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Specialized;
using ZoomParticipantChecker.Util;

namespace ZoomParticipantChecker.ViewModel.Tests
{
    [TestClass()]
    public class SettingDialogViewModelTests
    {

        [TestMethod()]
        [TestCategory("設定画面VM作成")]
        public void 設定画面VM作成()
        {
            var moq = new Mock<IConfigurationManager>();
            var settingDic = new NameValueCollection
            {
                { "MonitoringCycleMs", "100" },
            };
            moq.SetupGet(x => x.AppSettings).Returns(settingDic);
            AppSettingsManager.Intialization(moq.Object);

            var target = new SettingDialogViewModel();

            Assert.AreEqual("100", target.MonitoringCycleMs);
            Assert.AreEqual("", target.ExistsNotAppliedData);
        }

        [TestMethod()]
        [TestCategory("設定適用")]
        public void 設定適用()
        {
            var moq = new Mock<IConfigurationManager>();
            var settingDic = new NameValueCollection
            {
                { "MonitoringCycleMs", "100" },
            };
            moq.SetupGet(x => x.AppSettings).Returns(settingDic);
            AppSettingsManager.Intialization(moq.Object);

            var target = new SettingDialogViewModel();

            Assert.AreEqual("100", target.MonitoringCycleMs);
            Assert.AreEqual("", target.ExistsNotAppliedData);

            target.MonitoringCycleMs = "200";

            target.ApplyCommand.Execute(null);

            Assert.AreEqual("200", target.MonitoringCycleMs);
            Assert.AreEqual("※ 変更適用後再起動されていません。", target.ExistsNotAppliedData);
        }
    }
}