namespace SPM_WebConsole.Models
{
    internal static class App_Globals
    {
        private static string cfg_path;
        private static INIManager ini_manager;
        private static IWebHostEnvironment _webHostEnvironment;
        
        public static void Initialize(IServiceProvider serviceProvider) //Using Dependency injection
        {
            _webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();

            cfg_path = Path.Combine(_webHostEnvironment.ContentRootPath, "Config\\config.cfg");
            
            ini_manager = new INIManager(cfg_path);
        }

        //Получить значение по ключу name из секции APICONFIG
        public static string Url => ini_manager.GetPrivateString("APICONFIG", "api_hostname");
        public static string ApiKey => ini_manager.GetPrivateString("APICONFIG", "api_key");
        public static bool IsReadOnly => (ini_manager.GetPrivateString("APICONFIG", "readonly").ToLower() == "true") ? true : false;

        //Using for Reporting to pass universal format in parameters;
        public static string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";




    }
}
