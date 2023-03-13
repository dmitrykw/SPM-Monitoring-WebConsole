namespace SPM_WebConsole.Models.ViewModels.Reports
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

        public string DateTimeFormat => App_Globals.DateTimeFormat;
    }
}
