using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;

namespace SPM_WebClient.Models
{
    public class ReportsViewModel
    {
        public bool IsReadOnly { get { return App_Globals.IsReadOnly; } }

        public bool ApiIsAvailable { get; set; }
        public string ConnectionErrorHeader { get; set; } = "";

        
        public IEnumerable<ReportHost> ReportHosts { get; }

        public ReportsViewModel(IEnumerable<ReportHost> reportHosts) 
        {
            ReportHosts = reportHosts;            
        }
    }    
}