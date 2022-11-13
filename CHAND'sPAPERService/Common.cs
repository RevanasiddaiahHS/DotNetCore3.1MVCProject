using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;

namespace CHAND_sPAPERService
{
   public class Common
    {

        public static string contrainingString = GetConnenctionKey("CHANDsPAPER");

        public static string GetConnenctionKey(string key)
        {
            Configuration config = null;
            string ConfigValue = string.Empty;
            string exeConfigPath = typeof(Common).Assembly.Location;
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(exeConfigPath);
            }
            catch (Exception ex)
            {
                //handle errror here.. means DLL has no sattelite configuration file.
            }

            if (config != null)
            {
                ConfigValue = GetAppSetting(config, key);

            }
            return ConfigValue;
        }
        static string GetAppSetting(Configuration config, string key)
        {
            KeyValueConfigurationElement element = config.AppSettings.Settings[key];
            if (element != null)
            {
                string value = element.Value;
                if (!string.IsNullOrEmpty(value))
                    return value;
            }
            return string.Empty;
        }
       
    }
}
