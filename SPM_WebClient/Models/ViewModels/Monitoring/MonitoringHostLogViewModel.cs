using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SPM_WebClient.Models
{
    public class MonitoringHostLogViewModel
    {

        public List<KeyValuePair<int, string>> HostsLogs = new List<KeyValuePair<int, string>>();
        

        public MonitoringHostLogViewModel(int id)
        {
            FillHostLog(id);
        }


        
        private void FillHostLog(int id)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            {
                KeyValuePair<int?, string> hostlog = spm_api_processor.GetHostLog(id);
                if (hostlog.Key.HasValue && hostlog.Value != null)
                {

                    // string loghtmlstring = hostlog.Value.Replace("\r\n", @"<br />");

                    HostsLogs.Add(new KeyValuePair<int, string>(hostlog.Key.Value, hostlog.Value));
                }
            }
            catch
            { }
        }

    }
}