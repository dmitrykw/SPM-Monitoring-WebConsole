using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM_WebClient.Models
{
    public class LogicalDisc
    {
        public string Name { get; set; }
        public double TotalSpace { get; set; }
        public string TotalSpaceString
        {
            get
            {
                return TotalSpace.ToFormatedString();
            }
        }
        public double FreeSpace { get; set; }
        public string FreeSpaceString
        {
            get
            {
                return FreeSpace.ToFormatedString();
            }
        }
        public double UsedSpace
        {
            get
            {
                return TotalSpace - FreeSpace;
            }
        }
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


        public double UsedSpace_Percent
        {
            get
            {
                return (UsedSpace / TotalSpace) * 100;
            }
        }
        public string UsedSpace_PercentString { get { return UsedSpace_Percent.ToString().Replace(',', '.'); } }

        public string UsedSpace_progress_classname
        {
            get
            {
                if (UsedSpace_Percent > 85)
                { return "progress-bar-danger"; }
                else if (UsedSpace_Percent > 60)
                { return "progress-bar-warning"; }
                else
                { return "progress-bar-success"; }
            }
        }

    }
}