using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace SPM_WebClient.Models
{
    public class OptionsViewModel
    {        
        public Settings Settings = new Settings();

        string cfgpath;
        string url;
        string apikey;
        bool isreadonly;
        

        public bool IsReadOnly { get { return isreadonly; } }
        

        public bool ApiIsAvailable { get; set; }
        public string ConnectionErrorHeader = "";


        public OptionsViewModel()
        {
            GetConfig();

            FillSettings();            
            
        }


        private void GetConfig()
        {
            cfgpath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Config/config.cfg");
            INIManager manager = new INIManager(cfgpath);
            //Получить значение по ключу name из секции APICONFIG
            url = manager.GetPrivateString("APICONFIG", "api_hostname");
            apikey = manager.GetPrivateString("APICONFIG", "api_key");
            isreadonly = (manager.GetPrivateString("APICONFIG", "readonly").ToLower() == "true") ? true : false;
        }

        private void FillSettings()
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);
                           
                try
                {                  
                    Settings = spm_api_processor.GetSettings();
                    ApiIsAvailable = true;
                }
                catch {
                    ApiIsAvailable = false;
                    ConnectionErrorHeader = "You have API connection error. Check API configuration file (Config\\config.cfg)."; 
                }
            

        }

       
        
        public void SendOptionsUpdateAPI(UpdateSettingsObj input_data)
        {
            if (isreadonly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);                     
                try
                {spm_api_processor.SendSettingsUpdate(input_data);}
                catch(Exception ex) { throw ex;  }            
        }

    }
}