namespace SPM_WebConsole.Models.ViewModels.Monitoring
{
    public class MonitoringDetailsViewModel
    {

        public List<Host> Hosts = new List<Host>();


        public MonitoringDetailsViewModel(int id)
        {
            FillHosts(id);
        }


        private void FillHosts(int id)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            {
                Hosts = spm_api_processor.GetHosts(id);
            }
            catch
            { }
        }
    }
}
