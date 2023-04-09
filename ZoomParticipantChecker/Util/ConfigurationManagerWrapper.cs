using System.Collections.Specialized;
using System.Configuration;

namespace ZoomParticipantChecker.Util
{
    internal class ConfigurationManagerWrapper : IConfigurationManager
    {
        public NameValueCollection AppSettings => ConfigurationManager.AppSettings;

        public Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel)
            => ConfigurationManager.OpenExeConfiguration(userLevel);
    }
}
