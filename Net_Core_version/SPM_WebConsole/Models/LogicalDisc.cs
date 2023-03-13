namespace SPM_WebConsole.Models
{
    public class LogicalDisc
    {
        private const string css_bootstrap_progress_red = "progress-bar progress-bar-striped bg-danger";
        private const string css_bootstrap_progress_yellow = "progress-bar progress-bar-striped bg-warning";
        private const string css_bootstrap_progress_green = "progress-bar progress-bar-striped bg-success";

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
                { return css_bootstrap_progress_red; }
                else if (LoadPercent > 60)
                { return css_bootstrap_progress_yellow; }
                else
                { return css_bootstrap_progress_green; }
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
                { return css_bootstrap_progress_red; }
                else if (UsedSpace_Percent > 60)
                { return css_bootstrap_progress_yellow; }
                else
                { return css_bootstrap_progress_green; }
            }
        }

    }
}
