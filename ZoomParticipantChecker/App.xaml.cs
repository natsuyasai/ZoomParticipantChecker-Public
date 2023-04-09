using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using ZoomParticipantChecker.Model;
using ZoomParticipantChecker.Model.AutomationElementUtil;
using ZoomParticipantChecker.Util;
using ZoomParticipantChecker.ViewModel;

namespace ZoomParticipantChecker
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider Services { get; } = ConfigureServices();

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton<IConfigurationManager, ConfigurationManagerWrapper>()
                .AddSingleton<AutomationElementGetter>(provider => new AutomationElementGetter("参加者リスト"))
                .AddSingleton<MonitoringModel>()
                .AddSingleton<IMonitoringFacade, MonitoringFacade>()
                .AddTransient<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }

        public App()
        {
            string dicPath = (GetAppsUseLightTheme() == 0) ? @"Resource\Dark.xaml" : @"Resource\Light.xaml";
            var dic = new ResourceDictionary
            {
                Source = new Uri(dicPath, UriKind.Relative)
            };
            Resources.MergedDictionaries.Clear();
            Resources.MergedDictionaries.Add(dic);


            AppSettingsManager.Intialization(Services.GetRequiredService<IConfigurationManager>());
        }


        /// 
        /// AppsUseLightTheme の値を取得する。
        /// ダークモード：0 ライトモード：1 値がないなどのエラー：-1
        /// using Microsoft.Win32;()
        /// 
        private int GetAppsUseLightTheme()
        {
            int getmode = -1;
            string rKeyName = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
            string rGetValueName = "AppsUseLightTheme";
            try
            {
                var rKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(rKeyName);
                getmode = (int)rKey.GetValue(rGetValueName);

                // 開いたレジストリ・キーを閉じる
                rKey.Close();
            }
            catch (NullReferenceException)
            {
            }
            return getmode;
        }
    }
}
