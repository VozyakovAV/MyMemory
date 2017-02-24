using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace MyMemory.MVC
{
    public class Settings
    {
        private static Settings _instance;
        public static Settings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new Settings();
                }
                return _instance;
            }
        }

        public string GetSettings(string name)
        {
            return ConfigurationManager.AppSettings[name].ToString();
        }
    }
}