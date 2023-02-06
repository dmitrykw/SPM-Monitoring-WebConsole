namespace SPM_WebClient.Models
{
    internal static class App_Globals
    {
        private static string cfg_path;
        private static INIManager ini_manager;

        static App_Globals()
        {
            cfg_path = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Config/config.cfg");
            ini_manager = new INIManager(cfg_path);
        }

        //Получить значение по ключу name из секции APICONFIG
        public static string Url => ini_manager.GetPrivateString("APICONFIG", "api_hostname");
        public static string ApiKey => ini_manager.GetPrivateString("APICONFIG", "api_key");
        public static bool IsReadOnly => (ini_manager.GetPrivateString("APICONFIG", "readonly").ToLower() == "true") ? true : false;

        


    }
}