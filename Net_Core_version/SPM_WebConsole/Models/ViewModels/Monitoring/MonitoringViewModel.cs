using System.Text.RegularExpressions;

namespace SPM_WebConsole.Models.ViewModels.Monitoring
{
    public class MonitoringViewModel
    {
        public List<Host> Hosts = new List<Host>();
        public List<Group> Groups = new List<Group>();
        public int ShowHostsFilterLevel { get; private set; }



        public bool IsReadOnly { get { return App_Globals.IsReadOnly; } }

        public string GroupsHeader = "Groups:";

        public bool ApiIsAvailable { get; set; }
        public string ConnectionErrorHeader = "";

        public string SearchFieldText { get; set; }


        public MonitoringViewModel(string group_filter = "All Hosts", int hosts_status_filter_level = 0)
        {
            this.ShowHostsFilterLevel = hosts_status_filter_level;

            if (group_filter == null || group_filter == "") { FillGroups(); return; }


            FillHosts(group_filter, hosts_status_filter_level);
            FillGroups();

            Groups.Where(x => x.Name == group_filter).ToList().ForEach(c => c.IsActivated = true);


        }
        public MonitoringViewModel(string search_filter, bool is_search)
        {

            if (search_filter == null || search_filter == "") { FillGroups(); return; }

            FillHosts(search_filter, is_search);
            FillGroups();

            SearchFieldText = search_filter;
            //Groups.Where(x => x.Name == group_filter).ToList().ForEach(c => c.IsActivated = true);


        }

        public MonitoringViewModel(int host_id)
        {

            FillHosts(host_id);
            FillGroups();

        }


        public void SendHostUpdateAPI(UpdateHostObj model)
        {
            if (IsReadOnly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            { spm_api_processor.SendHostUpdate(model); }
            catch (Exception ex) { throw ex; }

        }

        public void RemoveHostAPI(int hostid_int)
        {
            if (IsReadOnly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            { spm_api_processor.RemoveHost(hostid_int); }
            catch (Exception ex) { throw ex; }
        }


        public void AddHostAPI(string hostname, string description, string groupname, string hosttype)
        {
            if (IsReadOnly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            { spm_api_processor.AddNewHost(hostname, description, groupname, hosttype); }
            catch (Exception ex) { throw ex; }
        }

        public void SendHostSortingUpdateAPI(UpdateHostSortingObj model)
        {
            if (IsReadOnly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            { spm_api_processor.SendHostSortingUpdate(model); }
            catch (Exception ex) { throw ex; }

        }

        public string SelectedGroupName
        {
            get
            {
                if (Groups.Where(x => x.IsActivated).Count() > 0)
                {
                    return Groups.Where(x => x.IsActivated).ToList().FirstOrDefault().Name;
                }
                else { return "nogroupselected"; }
            }
        }



        private void FillHosts(string group_filter, int show_hosts_filter_level = 0)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);

            try
            {
                if (group_filter != "All Hosts")
                { Hosts = spm_api_processor.GetHosts(group_filter); }
                else
                { Hosts = spm_api_processor.GetHosts(); }

                switch (show_hosts_filter_level)
                {
                    case 1:
                        Hosts = Hosts.Where(x => x.IsEnabled & x.Status & x.IsHostHaveSomeEvents).Concat(Hosts.Where(x => x.IsEnabled).Where(x => !x.Status)).ToList();
                        break;
                    case 2:
                        Hosts = Hosts.Where(x => x.IsEnabled).Where(x => !x.Status).ToList();
                        break;
                    default:
                        break;
                }

                ApiIsAvailable = true;
            }
            catch(Exception ex)
            {
                ApiIsAvailable = false;
                ConnectionErrorHeader = App_Globals.ApiConnectioErrorText + " " + ex.Message;
            }
        }


        private void FillHosts(string search_filter, bool is_search)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            {
                Hosts = spm_api_processor.GetHosts(search_filter, is_search);
                ApiIsAvailable = true;
            }
            catch (Exception ex)
            {
                ApiIsAvailable = false;
                ConnectionErrorHeader = App_Globals.ApiConnectioErrorText + " " + ex.Message;
            }
        }

        private void FillHosts(int id)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            {
                Hosts = spm_api_processor.GetHosts(id);
                ApiIsAvailable = true;
            }
            catch (Exception ex)
            {
                ApiIsAvailable = false;
                ConnectionErrorHeader = App_Globals.ApiConnectioErrorText + " " + ex.Message;
            }

        }

        private void FillGroups()
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            {
                Groups = spm_api_processor.GetGroups();
                ApiIsAvailable = false;
            }
            catch (Exception ex)
            {
                ApiIsAvailable = false;
                ConnectionErrorHeader = App_Globals.ApiConnectioErrorText + " " + ex.Message;
            }
        }

    }
}
