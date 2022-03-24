using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Web;
using System.Net;
using System.Text;
using System.IO;

namespace SPM_WebClient.Models
{
    public class MonitoringViewModel
    {
        public List<Host> Hosts = new List<Host>();
        public List<Group> Groups = new List<Group>();
        public int ShowHostsFilterLevel {get; private set;}
        string cfgpath;
        string url;
        string apikey;
        bool isreadonly;
        

        public bool IsReadOnly { get { return isreadonly; } }

        public string GroupsHeader = "Groups:";

        public bool ApiIsAvailable { get; set; }
        public string ConnectionErrorHeader = "";

        public string SearchFieldText { get; set; }

       
        public MonitoringViewModel(string group_filter = "All Hosts", int hosts_status_filter_level = 0)
        {
            this.ShowHostsFilterLevel = hosts_status_filter_level;
            GetConfig();

            if (group_filter == null || group_filter == "") { FillGroups(); return; }
            

            FillHosts(group_filter, hosts_status_filter_level);
            FillGroups();

            Groups.Where(x => x.Name == group_filter).ToList().ForEach(c => c.IsActivated = true);

           
        }
        public MonitoringViewModel(string search_filter, bool is_search)
        {
            GetConfig();

            if (search_filter == null || search_filter == "") { FillGroups(); return; }

            FillHosts(search_filter, is_search);
            FillGroups();

            SearchFieldText = search_filter;
            //Groups.Where(x => x.Name == group_filter).ToList().ForEach(c => c.IsActivated = true);


        }

        public MonitoringViewModel(int host_id)
        {
            GetConfig();            

            FillHosts(host_id);
            FillGroups();
           
        }


        public void SendHostUpdateAPI(UpdateHostObj model)
        {
            if (isreadonly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);
            try
            { spm_api_processor.SendHostUpdate(model); }
            catch (Exception ex) { throw ex; }

        }

        public void RemoveHostAPI(int hostid_int)
        {
            if (isreadonly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);
            try
            { spm_api_processor.RemoveHost(hostid_int); }
            catch (Exception ex) { throw ex; }
        }


        public void AddHostAPI(string hostname, string description, string groupname, string hosttype)
        {
            if (isreadonly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);
            try
            { spm_api_processor.AddNewHost(hostname, description, groupname, hosttype); }
            catch (Exception ex) { throw ex; }
        }

        public void SendHostSortingUpdateAPI(UpdateHostSortingObj model)
        {
            if (isreadonly) { return; }

            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);
            try
            { spm_api_processor.SendHostSortingUpdate(model); }
            catch (Exception ex) { throw ex; }

        }

        public string SelectedGroupName {
            get
            {
                if (Groups.Where(x => x.IsActivated).Count() > 0)
                {
                    return Groups.Where(x => x.IsActivated).ToList().FirstOrDefault().Name;
                }
                else { return "nogroupselected"; }
            }
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


        private void FillHosts(string group_filter, int show_hosts_filter_level = 0)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);

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
            catch
            {
                ApiIsAvailable = false;
                ConnectionErrorHeader = "You have API connection error. Check API configuration file (Config\\config.cfg).";
            }
        }


        private void FillHosts(string search_filter, bool is_search)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);                
                    try
                    {
                        Hosts = spm_api_processor.GetHosts(search_filter, is_search);
                        ApiIsAvailable = true;
                    }
                    catch
                    {
                        ApiIsAvailable = false;
                        ConnectionErrorHeader = "You have API connection error. Check API configuration file (Config\\config.cfg).";
                    }                            
        }

        private void FillHosts(int id)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);                     
                try
                {
                    Hosts = spm_api_processor.GetHosts(id);
                    ApiIsAvailable = true;
                }
                catch {
                    ApiIsAvailable = false;
                    ConnectionErrorHeader = "You have API connection error. Check API configuration file (Config\\config.cfg).";
                }            

        }

        private void FillGroups()
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);                        
                try
                {                
                    Groups = spm_api_processor.GetGroups();
                ApiIsAvailable = false;
                }
                catch 
                {
                    ApiIsAvailable = false;
                    ConnectionErrorHeader = "You have API connection error. Check API configuration file (Config\\config.cfg).";
                }            
        }

    }



    public class MonitoringDetailsViewModel
    {

        public List<Host> Hosts = new List<Host>();        
        string cfgpath;
        string url;
        string apikey;

        public MonitoringDetailsViewModel(int id)
        {
            GetConfig();

            FillHosts(id);
                 
        }




        private void GetConfig()
        {
            cfgpath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Config/config.cfg");
            INIManager manager = new INIManager(cfgpath);
            //Получить значение по ключу name из секции APICONFIG
            url = manager.GetPrivateString("APICONFIG", "api_hostname");
            apikey = manager.GetPrivateString("APICONFIG", "api_key");
        }

        private void FillHosts(int id)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);
            try
            {
                Hosts = spm_api_processor.GetHosts(id);             
            }
            catch
            {}

        }

    }


    public class MonitoringHostLogViewModel
    {

        public List<KeyValuePair<int, string>> HostsLogs = new List<KeyValuePair<int, string>>();
        string cfgpath;
        string url;
        string apikey;

        public MonitoringHostLogViewModel(int id)
        {
            GetConfig();

            FillHostLog(id);            
                 
        }




        private void GetConfig()
        {
            cfgpath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/Config/config.cfg");
            INIManager manager = new INIManager(cfgpath);
            //Получить значение по ключу name из секции APICONFIG
            url = manager.GetPrivateString("APICONFIG", "api_hostname");
            apikey = manager.GetPrivateString("APICONFIG", "api_key");
        }

        private void FillHostLog(int id)
        {
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(url, apikey);
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
            {}
            //http://localhost:51634/Monitoring/HostLog?id=211656325
        }

    }
}