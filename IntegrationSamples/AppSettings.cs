using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;

namespace IntegrationSamples
{
    public static class AppSettings
    {

        public static string AzureStorageConnectionString
        {
            get
            {
                return Setting<string>("vbuh_AzureStorageConnectionString");
            }
        }

        public static string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["SQLSERVER"].ConnectionString;
            }
        }

        public static string verify_token
        {
            get
            {
                return Setting<string>("Facebook:verify_token");
            }
        }

        public static string access_token
        {
            get
            {
                return Setting<string>("Facebook:access_token");
            }
        }

        public static string ClientID
        {
            get
            {
                return Setting<string>("Purecloud:ClientID");
            }
        }

        public static string ClientSecret
        {
            get
            {
                return Setting<string>("Purecloud:ClientSecret");
            }
        }

        public static string RedirectUri
        {
            get
            {
                return Setting<string>("Purecloud:RedirectUri");
            }
        }

        private static T Setting<T>(string name)
        {
            string value = ConfigurationManager.AppSettings[name];

            if (value == null)
            {
                throw new Exception(String.Format("Could not find setting '{0}',", name));
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}