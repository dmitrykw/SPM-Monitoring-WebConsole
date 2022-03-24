using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM_WebClient.Models
{
    public class NetworkAdapter
    {
        public string Name { get; set; }
        public double LoadPercent { get; set; }
        public string LoadPercentString { get { return LoadPercent.ToString().Replace(',', '.'); } }

        public string Load_progress_classname
        {
            get
            {
                if (LoadPercent > 85)
                { return "progress-bar-danger"; }
                else if (LoadPercent > 60)
                { return "progress-bar-warning"; }
                else
                { return "progress-bar-success"; }
            }
        }
    }
}