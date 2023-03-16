using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
namespace SPM_WebConsole.Models
{
    internal static class App_Globals
    {
        //private static string _cfg_path;
        //private static INIManager _ini_manager;
        private static IWebHostEnvironment _webHostEnvironment;
        private static IConfiguration _appConfiguration;
        

        public static void Initialize(IServiceProvider serviceProvider, IConfiguration appConfiguration) //Using Dependency injection
        {
            _webHostEnvironment = serviceProvider.GetRequiredService<IWebHostEnvironment>();
            _appConfiguration = appConfiguration;

            //_cfg_path = Path.Combine(_webHostEnvironment.ContentRootPath, "Config/config.cfg");
            //_ini_manager = new INIManager(_cfg_path);
        }

        //Получить значение по ключу name из секции APICONFIG
        //public static string Url => _ini_manager.GetPrivateString("APICONFIG", "api_hostname");
        //public static string ApiKey => _ini_manager.GetPrivateString("APICONFIG", "api_key");
        //public static bool IsReadOnly => (_ini_manager.GetPrivateString("APICONFIG", "readonly").ToLower() == "true") ? true : false;
        public static string Url => _appConfiguration.GetValue<string>("api_hostname");
        public static string ApiKey => _appConfiguration.GetValue<string>("api_key");
        public static bool IsReadOnly => _appConfiguration.GetValue<bool>("readonly");


        //Using for Reporting to pass universal format in parameters;
        public static string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
        public static string ApiConnectioErrorText = "SPM Monitoring API connection error. Check API configuration file (Config/spm_api_settings.json).";
    }
}
