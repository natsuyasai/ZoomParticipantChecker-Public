using System.Collections.Specialized;
using System.Configuration;

namespace ZoomParticipantChecker.Util
{
    public interface IConfigurationManager
    {
        NameValueCollection AppSettings
        {
            get;
        }

        Configuration OpenExeConfiguration(ConfigurationUserLevel userLevel);
    }
}