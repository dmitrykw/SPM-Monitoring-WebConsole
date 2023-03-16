using Newtonsoft.Json;
using SPM_WebConsole.Models.ViewModels.Reports;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.PortableExecutable;
using System.Text;

namespace SPM_WebConsole.Models
{
    public class Spm_Api_Processor
    {
        private readonly string _url;
        private readonly string _apikey;

        public Spm_Api_Processor(string url, string apikey)
        {
            this._apikey = apikey;
            this._url = url;
        }


        public Settings GetSettings()
        {
            var result = new Settings();

            string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Options?api_token=" + _apikey).Result;

            if (JSON != null)
            { result = JsonConvert.DeserializeObject<Settings>(JSON); }

            return result ?? new Settings();
        }





        public void SendSettingsUpdate(UpdateSettingsObj input_data)
        {
            try
            {
                var NotificationWeekDays = new List<KeyValuePair<string, bool?>>
                {
                    new KeyValuePair<string, bool?>("Monday", input_data.notifyonmonday),
                    new KeyValuePair<string, bool?>("Tuesday", input_data.notifyontuesday),
                    new KeyValuePair<string, bool?>("Wednesday", input_data.notifyonwednesday),
                    new KeyValuePair<string, bool?>("Thursday", input_data.notifyonthursday),
                    new KeyValuePair<string, bool?>("Friday", input_data.notifyonmfriday),
                    new KeyValuePair<string, bool?>("Saturday", input_data.notifyonsaturday),
                    new KeyValuePair<string, bool?>("Sunday", input_data.notifyonsunday)
                };

                //JSON = JsonConvert.SerializeObject(new { ID = hostid_int, Hostname = hostname, Description = description, GroupName = groupname, IsNotifyEnabled = isnotifyenabled.HasValue ? isnotifyenabled.Value : false, IsEnabled = isenabled.HasValue ? isenabled.Value : false });
                string JSON = JsonConvert.SerializeObject(
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


                var headers = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("api_token", _apikey),
                    };


                try
                {
                    _ = SendRequestAsync(HttpMethod.Put, _url + "/api/Options", JSON, headers).Result;

                }
                catch { throw new Exception("Api Connection Error in SendSettingsUpdate in Spm_Api_Processor. API: Update data PUT Action. Error when get response from server."); }

            }
            catch { throw new Exception("Api Connection Error in SendSettingsUpdate in Spm_Api_Processor. API Update data PUT Action."); }

        }



        public List<Host> GetHosts()
        {
            var result = new List<Host>();

            string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Hosts?api_token=" + _apikey).Result;

            if (JSON != null)
            { result = JsonConvert.DeserializeObject<List<Host>>(JSON); }

            return result ?? new List<Host>();
        }



        public List<Host> GetHosts(string group_filter)
        {
            var result = new List<Host>();

            string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Groups?name=" + group_filter + "&api_token=" + _apikey).Result;

            if (JSON != null)
            { result = JsonConvert.DeserializeObject<List<Host>>(JSON); }

            return result ?? new List<Host>();
        }


        public List<Host> GetHosts(string search_filter, bool is_search)
        {
            var result = new List<Host>();

            if (is_search)
            {
                string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Hosts?name=" + search_filter + "&is_part_of_name=true&api_token=" + _apikey).Result;

                if (JSON != null)
                { result = JsonConvert.DeserializeObject<List<Host>>(JSON); }
            }
            return result ?? new List<Host>();

        }


        public List<Host> GetHosts(int id)
        {
            var result = new List<Host>();

            string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Hosts?id=" + id + "&api_token=" + _apikey).Result;

            if (JSON != null)
            { result = JsonConvert.DeserializeObject<List<Host>>(JSON); }

            return result ?? new List<Host>();

        }


        public List<Group> GetGroups()
        {
            var result = new List<Group>();

            string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Groups?api_token=" + _apikey).Result;

            if (JSON != null)
            { result = JsonConvert.DeserializeObject<List<Group>>(JSON); }

            return result ?? new List<Group>();

        }

        public KeyValuePair<int?, string> GetHostLog(int id)
        {
            var result = new KeyValuePair<int?, string>();

            string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Logs?id=" + id + "&api_token=" + _apikey).Result;

            if (JSON != null)
            { result = JsonConvert.DeserializeObject<KeyValuePair<int?, string>>(JSON); }

            return result;
        }




        public void SendHostUpdate(UpdateHostObj model)
        {
            try
            {
                bool parse_result = int.TryParse(model.hostid, out int hostid_int);
                if (!parse_result) { hostid_int = 999999999; }

                //Host Custom Settings List to send
                var HostCustomOptions = new List<KeyValuePair<string, dynamic>>
                {
                    new KeyValuePair<string, dynamic>("isEnabledCustomNotificationSettings", model.isenabledcustomnotificationsettings),
                    new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_CPU_is_Overload", model.customagentnotifyifcpuisoverload),
                    new KeyValuePair<string, dynamic>("Custom_Agent_CPU_Overload_Percent", model.customagentcpuoverloadpercent),
                    new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_LowFreeMem", model.customagentnotifyiflowfreemem),
                    new KeyValuePair<string, dynamic>("Custom_Agent_LowFreeMem_Megabytes", model.customagentlowfreememmegabytes),
                    new KeyValuePair<string, dynamic>("Custom_Agent_Notify_LowDisksFreeSpace", model.customagentnotifylowdisksfreespace),
                    new KeyValuePair<string, dynamic>("Custom_Agent_LowDisksFreeSpace_Megabytes", model.customagentlowdisksfreespacemegabytes),
                    new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_Disks_Overload", model.customagentnotifyifdisksoverload),
                    new KeyValuePair<string, dynamic>("Custom_Agent_DisksOverload_Percent", model.customagentdisksoverloadpercent),
                    new KeyValuePair<string, dynamic>("Custom_Agent_Notify_if_NetAdapters_Overload", model.customagentnotifyifnetadaptersoverload),
                    new KeyValuePair<string, dynamic>("Custom_Agent_NetAdaptersOverload_Percent", model.customagentnetadaptersoverloadpercent),
                    new KeyValuePair<string, dynamic>("Custom_Notify_if_AgentConnection_Lost", model.customnotifyifagentconnectionlost),
                    new KeyValuePair<string, dynamic>("isEnabledCustomOtherNotificationSettings", model.isenabledcustomothernotificationsettings),
                    new KeyValuePair<string, dynamic>("Custom_Notify_if_AnswerTime_IsLong", model.customnotifyifanswertimeislong),
                    new KeyValuePair<string, dynamic>("Custom_LongAnswerTime_miliseconds", model.customlonganswertimemiliseconds),
                    new KeyValuePair<string, dynamic>("Custom_EventLOG_Notify_if_ComputerRestarted", model.customeventlognotifyifcomputerrestarted),
                    new KeyValuePair<string, dynamic>("Custom_EventLOG_Notify_if_ComputerGoingReboot", model.customeventlognotifyifcomputergoingreboot),
                    new KeyValuePair<string, dynamic>("Custom_EventLOG_Notify_if_ComputerRebootedByUser", model.customeventlognotifyifcomputerrebootedbyuser),
                    new KeyValuePair<string, dynamic>("Custom_EventLOG_ForwardCriticalEvents", model.customeventlogforwardcriticalevents),
                    new KeyValuePair<string, dynamic>("Custom_EventLOG_ForwardErrorEvents", model.customeventlogforwarderrorevents),
                    new KeyValuePair<string, dynamic>("isEnabledCustomEmail", model.isenabledcustomemail),
                    new KeyValuePair<string, dynamic>("Custom_Email", model.customemail),
                    new KeyValuePair<string, dynamic>("HostDependentOn", model.hostdepending),
                    new KeyValuePair<string, dynamic>("Custom_UseSNMP", model.isenabledsnmp)
                };


                var CustomScheduledWeekDays = new List<KeyValuePair<string, bool?>>
                {
                    new KeyValuePair<string, bool?>("Monday", model.notifyonmonday),
                    new KeyValuePair<string, bool?>("Tuesday", model.notifyontuesday),
                    new KeyValuePair<string, bool?>("Wednesday", model.notifyonwednesday),
                    new KeyValuePair<string, bool?>("Thursday", model.notifyonthursday),
                    new KeyValuePair<string, bool?>("Friday", model.notifyonmfriday),
                    new KeyValuePair<string, bool?>("Saturday", model.notifyonsaturday),
                    new KeyValuePair<string, bool?>("Sunday", model.notifyonsunday)
                };
                HostCustomOptions.Add(new KeyValuePair<string, dynamic>("Custom_Notification_WeekDays", CustomScheduledWeekDays));

                //Resulted JSON object to send
                string JSON = JsonConvert.SerializeObject(new
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



                var headers = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("api_token", _apikey),
                    };


                try
                {
                    _ = SendRequestAsync(HttpMethod.Put, _url + "/api/Hosts", JSON, headers).Result;
                }
                catch { throw new Exception("Api Connection Error in SendHostUpdate in Spm_Api_Processor. API: Update data PUT Action. Error when get response from server."); }

            }
            catch { throw new Exception("Api Connection Error in SendHostUpdate in Spm_Api_Processor. API: Update data PUT Action."); }

        }

        public void AddNewHost(string hostname, string description, string groupname, string hosttype)
        {
            try
            {
                string JSON = JsonConvert.SerializeObject(new { Hostname = hostname, Description = description, GroupName = groupname, HostType = hosttype });

                var headers = new List<KeyValuePair<string, string>>()
                {
                        new KeyValuePair<string, string>("api_token", _apikey),
                };

                try
                {
                    _ = SendRequestAsync(HttpMethod.Post, _url + "/api/Hosts", JSON, headers).Result;
                }
                catch { throw new Exception("Api Connection Error in AddNewHost in Spm_Api_Processor. API: Add new host POST Action. Error when get response from server."); }

            }
            catch { throw new Exception("Api Connection Error in AddNewHost in Spm_Api_Processor. API: Add new host POST Action."); }
        }


        public void RemoveHost(int hostid_int)
        {
            try
            {
                string JSON = JsonConvert.SerializeObject(new { ID = hostid_int });

                var headers = new List<KeyValuePair<string, string>>()
                {
                        new KeyValuePair<string, string>("api_token", _apikey),
                };

                try
                {
                    _ = SendRequestAsync(HttpMethod.Delete, _url + "/api/Hosts", JSON, headers).Result;
                }
                catch { throw new Exception("Api Connection Error in RemoveHost in Spm_Api_Processor. API: Remove host DELETE Action. Error when get response from server."); }

            }
            catch { throw new Exception("Api Connection Error in RemoveHost in Spm_Api_Processor. API: Remove host DELETE Action."); }

        }

        public void SendHostSortingUpdate(UpdateHostSortingObj model)
        {
            try
            {
                //Resulted JSON object to send
                string JSON = JsonConvert.SerializeObject(new
                {
                    Host_ID_Sequence = model.ids_string
                });

                var headers = new List<KeyValuePair<string, string>>()
                {
                        new KeyValuePair<string, string>("api_token", _apikey),
                };
                try
                {
                    _ = SendRequestAsync(HttpMethod.Post, _url + "/api/HostsSorting", JSON, headers).Result;
                }
                catch { throw new Exception("Api Connection Error in SendHostSortingUpdate in Spm_Api_Processor. API: Update data POST Action. Error when get response from server."); }

            }
            catch { throw new Exception("Api Connection Error in SendHostSortingUpdate in Spm_Api_Processor. API: Update data POST Action."); }
        }


        public List<ReportHost> GetReportHosts(DateTime load_date_from, DateTime load_date_to, Operators load_answer_time_sign, int load_answer_time, string[] load_hostnames, bool load_failed_only, bool load_auto_scaling, int load_scaling_index)
        {
            var result = new List<ReportHost>();

            string reportParams = JsonConvert.SerializeObject(new
            {
                load_date_from,
                load_date_to,
                load_answer_time_sign,
                load_answer_time,
                load_hostnames,
                load_failed_only,
                load_auto_scaling,
                load_scaling_index
            });


            var headers = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("api_token", _apikey),
                new KeyValuePair<string, string>("params_json", reportParams)

            };

            string? JSON = SendRequestAsync(HttpMethod.Get, _url + "/api/Reports", html_headers: headers).Result;

            if (JSON != null)
            { result = JsonConvert.DeserializeObject<List<ReportHost>>(JSON); }

            return result ?? new List<ReportHost>();
        }



        private static async Task<string?> SendRequestAsync(HttpMethod httpMethod, string user_request, string user_request_content = null!, IEnumerable<KeyValuePair<string, string>> html_headers = null!)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(Encoding.UTF8.WebName));

                using (var request = new HttpRequestMessage(httpMethod, user_request))
                {
                    if (user_request_content != null)
                    {
                        request.Content = new StringContent(user_request_content, Encoding.UTF8, "application/json");
                    }

                    if (html_headers != null && html_headers.Any())
                    {
                        foreach (var header in html_headers)
                        {
                            request.Headers.Add(header.Key, header.Value);
                        }
                    }

                    var response = await httpClient.SendAsync(request);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
