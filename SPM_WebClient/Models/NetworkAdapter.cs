using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM_WebClient.Models
{
    public class NetworkAdapter
    {
        private const string css_bootstrap_progress_red = "progress-bar progress-bar-striped bg-danger";
        private const string css_bootstrap_progress_yellow = "progress-bar progress-bar-striped bg-warning";
        private const string css_bootstrap_progress_green = "progress-bar progress-bar-striped bg-success";
                

        public string Name { get; set; }
        public double LoadPercent { get; set; }
        public string LoadPercentString { get { return LoadPercent.ToString().Replace(',', '.'); } }

        public string Load_progress_classname
        {
            get
            {
                if (LoadPercent > 85)
                { return css_bootstrap_progress_red; }
                else if (LoadPercent > 60)
                { return css_bootstrap_progress_yellow; }
                else
                { return css_bootstrap_progress_green; }
            }
        }
    }
}