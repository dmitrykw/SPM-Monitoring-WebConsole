using Newtonsoft.Json;
using SPM_WebConsole.Models.ViewModels.Reports;
using System.Net;
using System.Text;

namespace SPM_WebConsole.Models
{
    public class Spm_Api_Processor
    {
        string url;
        string apikey;

        public Spm_Api_Processor(string url, string apikey)
        {
            this.apikey = apikey;
            this.url = url;
        }


        public Settings GetSettings()
        {
            Settings result = new Settings();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string JSON = wc.DownloadString(url + "/api/Options?api_token=" + apikey);

                result = JsonConvert.DeserializeObject<Settings>(JSON);
            }

            return result;
        }


        public void SendSettingsUpdate(UpdateSettingsObj input_data)
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Options"); //- Токен надо передать в header е

                    request.Method = "PUT";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);



                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);


                    List<KeyValuePair<string, bool?>> NotificationWeekDays = new List<KeyValuePair<string, bool?>>();
                    NotificationWeekDays.Add(new KeyValuePair<string, bool?>("Monday", input_data.notifyonmonday));
                    NotificationWeekDays.Add(new KeyValuePair<string, bool?>("Tuesday", input_data.notifyontuesday));
                    NotificationWeekDays.Add(new KeyValuePair<string, bool?>("Wednesday", input_data.notifyonwednesday));
                    NotificationWeekDays.Add(new KeyValuePair<string, bool?>("Thursday", input_data.notifyonthursday));
                    NotificationWeekDays.Add(new KeyValuePair<string, bool?>("Friday", input_data.notifyonmfriday));
                    NotificationWeekDays.Add(new KeyValuePair<string, bool?>("Saturday", input_data.notifyonsaturday));
                    NotificationWeekDays.Add(new KeyValuePair<string, bool?>("Sunday", input_data.notifyonsunday));

                    //JSON = JsonConvert.SerializeObject(new { ID = hostid_int, Hostname = hostname, Description = description, GroupName = groupname, IsNotifyEnabled = isnotifyenabled.HasValue ? isnotifyenabled.Value : false, IsEnabled = isenabled.HasValue ? isenabled.Value : false });
                    JSON = JsonConvert.SerializeObject(
                        new
                        {
                            Enable_Email_Notifications = input_data.notifybyemail,
                            Enable_TelegramBot_Notifications = input_data.notifybytelegram,
                            Enable_SMS_Notifications = input_data.notifybysms,
                            Enable_Push_Notifications = input_data.notifybypush,
                            Hosts_Query_Interval_seconds = input_data.hostsqueryinterval,
                            ICMP_TimeOut_miliseconds = input_data.icmptimeout,
                            HTTP_TimeOut_miliseconds = input_data.httptimeout,
                            Number_Of_Fails_Before_Send_Notification = input_data.failsbeforenotify,
                            Write_Hosts_Log = input_data.writelog,
                            Write_Log_For_Each_Host = input_data.writelogforeachhost,
                            Hosts_Visual_Type = input_data.hosttype,
                            Monitor_Picture_Type = input_data.hostmonitortype,
                            Enable_Query_ControlHost = input_data.enablecontrolhost,
                            ControlHost_URI = input_data.controlhosturi,
                            Enable_ControlHost_Notifications = input_data.enablecontrolhostnotify,
                            Agent_Measure_Time_Before_Sent_Notifications_minutes = input_data.agentmeasurestime,
                            Agent_Notify_if_CPU_is_Overload = input_data.notifycpuoverload,
                            Agent_CPU_Overload_Percent = input_data.notifycpuoverloadpercent,
                            Agent_Notify_if_LowFreeMem = input_data.notifylowfreemem,
                            Agent_LowFreeMem_Megabytes = input_data.notifylowfreememmegabytes,
                            Agent_Notify_LowDisksFreeSpace = input_data.notifylowdisksspace,
                            Agent_LowDisksFreeSpace_Megabytes = input_data.notifylowdisksspacemegabytes,
                            Agent_Notify_if_Disks_Overload = input_data.notifydisksoverload,
                            Agent_DisksOverload_Percent = input_data.notifydisksoverloadpercent,
                            Agent_Notify_if_NetAdapters_Overload = input_data.notifynetoverload,
                            Agent_NetAdaptersOverload_Percent = input_data.notifynetoverloadpercent,
                            Agent_Notify_AgentConnectionLost = input_data.notifyagentlost,
                            Notify_if_AnswerTime_IsLong = input_data.notifylonganswertime,
                            LongAnswerTime_miliseconds = input_data.longanswertime,
                            EventLOG_Notify_if_ComputerRestarted = input_data.notifyhostrestarted,
                            EventLOG_Notify_if_ComputerGoingReboot = input_data.notifyhostgoingreboot,
                            EventLOG_Notify_if_ComputerRebootedByUser = input_data.notifyrebootbyuser,
                            EventLOG_ForwardCriticalEvents = input_data.forwardcriticalevents,
                            EventLOG_ForwardErrorEvents = input_data.forwarderrorevents,
                            NotificationWeekDays
                        });

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                        //     Stream webStream = webResponse.GetResponseStream();
                        //      StreamReader responseReader = new StreamReader(webStream);
                        //     string response = responseReader.ReadToEnd();
                        //     responseReader.Close();
                    }
                    catch { throw new Exception("WebClient Api Connection Error in Options Controller. API: Update data PUT Action. Error when get response from server."); }

                }
                catch { throw new Exception("WebClient Api Connection Error in Options Controller. API Update data PUT Action."); }
            }
        }



        public List<Host> GetHosts()
        {
            List<Host> result = new List<Host>();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string JSON = wc.DownloadString(url + "/api/Hosts?api_token=" + apikey);

                result = JsonConvert.DeserializeObject<List<Host>>(JSON);
            }

            return result;
        }

        public List<Host> GetHosts(string group_filter)
        {
            List<Host> result = new List<Host>();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;

                string JSON = wc.DownloadString(url + "/api/Groups?name=" + group_filter + "&api_token=" + apikey);

                result = JsonConvert.DeserializeObject<List<Host>>(JSON);
            }

            return result;
        }


        public List<Host> GetHosts(string search_filter, bool is_search)
        {
            List<Host> result = new List<Host>();

            if (is_search)
            {
                using (WebClient wc = new WebClient())
                {

                    wc.Encoding = Encoding.UTF8;

                    string JSON = wc.DownloadString(url + "/api/Hosts?name=" + search_filter + "&is_part_of_name=true&api_token=" + apikey);

                    result = JsonConvert.DeserializeObject<List<Host>>(JSON);
                }
            }

            return result;
        }


        public List<Host> GetHosts(int id)
        {
            List<Host> result = new List<Host>();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;
                string JSON = wc.DownloadString(url + "/api/Hosts?id=" + id + "&api_token=" + apikey);

                result = JsonConvert.DeserializeObject<List<Host>>(JSON);
            }

            return result;
        }


        public List<Group> GetGroups()
        {
            List<Group> result = new List<Group>();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;

                string JSON = wc.DownloadString(url + "/api/Groups?api_token=" + apikey);

                result = JsonConvert.DeserializeObject<List<Group>>(JSON);
            }

            return result;
        }

        public KeyValuePair<int?, string> GetHostLog(int id)
        {
            KeyValuePair<int?, string> result = new KeyValuePair<int?, string>();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;

                string JSON = wc.DownloadString(url + "/api/Logs?id=" + id + "&api_token=" + apikey);

                result = JsonConvert.DeserializeObject<KeyValuePair<int?, string>>(JSON);
            }

            return result;
        }




        public void SendHostUpdate(UpdateHostObj model)
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Hosts"); //- Токен надо передать в header е

                    request.Method = "PUT";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);



                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);

                    int hostid_int = 0;
                    int.TryParse(model.hostid, out hostid_int);

                    //Host Custom Settings List to send
                    List<KeyValuePair<string, dynamic>> HostCustomOptions = new List<KeyValuePair<string, dynamic>>();
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("isEnabledCustomNotificationSettings", model.isenabledcustomnotificationsettings));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_CPU_is_Overload", model.customagentnotifyifcpuisoverload));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_CPU_Overload_Percent", model.customagentcpuoverloadpercent));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_LowFreeMem", model.customagentnotifyiflowfreemem));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_LowFreeMem_Megabytes", model.customagentlowfreememmegabytes));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_Notify_LowDisksFreeSpace", model.customagentnotifylowdisksfreespace));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_LowDisksFreeSpace_Megabytes", model.customagentlowdisksfreespacemegabytes));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_Disks_Overload", model.customagentnotifyifdisksoverload));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_DisksOverload_Percent", model.customagentdisksoverloadpercent));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_NetAdapters_Overload", model.customagentnotifyifnetadaptersoverload));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Agent_NetAdaptersOverload_Percent", model.customagentnetadaptersoverloadpercent));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Notify_if_AgentConnection_Lost", model.customnotifyifagentconnectionlost));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("isEnabledCustomOtherNotificationSettings", model.isenabledcustomothernotificationsettings));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Notify_if_AnswerTime_IsLong", model.customnotifyifanswertimeislong));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_LongAnswerTime_miliseconds", model.customlonganswertimemiliseconds));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_EventLOG_Notify_if_ComputerRestarted", model.customeventlognotifyifcomputerrestarted));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_EventLOG_Notify_if_ComputerGoingReboot", model.customeventlognotifyifcomputergoingreboot));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_EventLOG_Notify_if_ComputerRebootedByUser", model.customeventlognotifyifcomputerrebootedbyuser));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_EventLOG_ForwardCriticalEvents", model.customeventlogforwardcriticalevents));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_EventLOG_ForwardErrorEvents", model.customeventlogforwarderrorevents));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("isEnabledCustomEmail", model.isenabledcustomemail));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Email", model.customemail));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("HostDependentOn", model.hostdepending));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_UseSNMP", model.isenabledsnmp));


                    List<KeyValuePair<string, bool?>> CustomScheduledWeekDays = new List<KeyValuePair<string, bool?>>();
                    CustomScheduledWeekDays.Add(new KeyValuePair<string, bool?>("Monday", model.notifyonmonday));
                    CustomScheduledWeekDays.Add(new KeyValuePair<string, bool?>("Tuesday", model.notifyontuesday));
                    CustomScheduledWeekDays.Add(new KeyValuePair<string, bool?>("Wednesday", model.notifyonwednesday));
                    CustomScheduledWeekDays.Add(new KeyValuePair<string, bool?>("Thursday", model.notifyonthursday));
                    CustomScheduledWeekDays.Add(new KeyValuePair<string, bool?>("Friday", model.notifyonmfriday));
                    CustomScheduledWeekDays.Add(new KeyValuePair<string, bool?>("Saturday", model.notifyonsaturday));
                    CustomScheduledWeekDays.Add(new KeyValuePair<string, bool?>("Sunday", model.notifyonsunday));
                    HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Notification_WeekDays", CustomScheduledWeekDays));

                    //Resulted JSON object to send
                    JSON = JsonConvert.SerializeObject(new
                    {
                        ID = hostid_int,
                        Hostname = model.hostname,
                        Description = model.description,
                        GroupName = model.groupname,
                        IsNotifyEnabled = model.isnotifyenabled,
                        IsEnabled = model.isenabled,
                        ImgPath = model.imgpath,
                        HostCustomOptions
                    });

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                        //     Stream webStream = webResponse.GetResponseStream();
                        //      StreamReader responseReader = new StreamReader(webStream);
                        //     string response = responseReader.ReadToEnd();
                        //     responseReader.Close();
                    }
                    catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Update data PUT Action. Error when get response from server."); }

                }
                catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Update data PUT Action."); }
            }
        }

        public void AddNewHost(string hostname, string description, string groupname, string hosttype)
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Hosts"); //- Токен надо передать в header е

                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);



                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);

                    JSON = JsonConvert.SerializeObject(new { Hostname = hostname, Description = description, GroupName = groupname, HostType = hosttype });

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                        //     Stream webStream = webResponse.GetResponseStream();
                        //      StreamReader responseReader = new StreamReader(webStream);
                        //     string response = responseReader.ReadToEnd();
                        //     responseReader.Close();
                    }
                    catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Add new host POST Action. Error when get response from server."); }

                }
                catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Add new host POST Action."); }
            }
        }


        public void RemoveHost(int hostid_int)
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/Hosts"); //- Токен надо передать в header е

                    request.Method = "DELETE";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);



                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);

                    JSON = JsonConvert.SerializeObject(new { ID = hostid_int });

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                        //     Stream webStream = webResponse.GetResponseStream();
                        //      StreamReader responseReader = new StreamReader(webStream);
                        //     string response = responseReader.ReadToEnd();
                        //     responseReader.Close();
                    }
                    catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Remove host DELETE Action. Error when get response from server."); }

                }
                catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Remove host DELETE Action."); }
            }
        }

        public void SendHostSortingUpdate(UpdateHostSortingObj model)
        {
            using (WebClient wc = new WebClient())
            {
                string JSON = "";
                try
                {


                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + "/api/HostsSorting"); //- Токен надо передать в header е

                    request.Method = "POST";
                    request.ContentType = "application/json; charset=utf-8";
                    request.Accept = "application/json; charset=utf-8";
                    request.Headers.Add("api_token", apikey);



                    StreamWriter requestWriter = new StreamWriter(request.GetRequestStream(), Encoding.UTF8);


                    //Resulted JSON object to send
                    JSON = JsonConvert.SerializeObject(new
                    {
                        Host_ID_Sequence = model.ids_string
                    });

                    requestWriter.Write(JSON);

                    requestWriter.Close();


                    try
                    {
                        WebResponse webResponse = request.GetResponse();

                        //     Stream webStream = webResponse.GetResponseStream();
                        //      StreamReader responseReader = new StreamReader(webStream);
                        //     string response = responseReader.ReadToEnd();
                        //     responseReader.Close();
                    }
                    catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Update data PUT Action. Error when get response from server."); }

                }
                catch { throw new Exception("WebClient Api Connection Error in Monitoring Controller. API: Update data PUT Action."); }
            }
        }


        public List<ReportHost> GetReportHosts(DateTime load_date_from, DateTime load_date_to, Operators load_answer_time_sign, int load_answer_time, string[] load_hostnames, bool load_failed_only, bool load_auto_scaling, int load_scaling_index)
        {
            List<ReportHost> result = new List<ReportHost>();

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = Encoding.UTF8;

                string reportParams = JsonConvert.SerializeObject(new
                {
                    load_date_from = load_date_from,
                    load_date_to = load_date_to,
                    load_answer_time_sign = load_answer_time_sign,
                    load_answer_time = load_answer_time,
                    load_hostnames = load_hostnames,
                    load_failed_only = load_failed_only,
                    load_auto_scaling = load_auto_scaling,
                    load_scaling_index = load_scaling_index
                });

                wc.Headers.Add("api_token", apikey);
                wc.Headers.Add("params_json", reportParams);

                string JSON = wc.DownloadString(url + "/api/Reports");

                result = JsonConvert.DeserializeObject<List<ReportHost>>(JSON);
            }

            return result;
        }
    }
}
