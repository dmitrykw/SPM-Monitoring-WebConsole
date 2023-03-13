using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPM_WebClient.Models
{

    public class UpdateSettingsObj
    {

        public int? hosttype { get; set; }
        public int? hostmonitortype { get; set; }
        public int? hostsqueryinterval { get; set; }


        public int? icmptimeout { get; set; }
        public int? httptimeout { get; set; }
        public bool? enablecontrolhost { get; set; }
        public string controlhosturi { get; set; }
        public bool? enablecontrolhostnotify { get; set; }
        public int? failsbeforenotify { get; set; }
        public bool? notifybyemail { get; set; }
        public bool? notifybysms { get; set; }
        public bool? notifybypush { get; set; }        
        public bool? notifybytelegram { get; set; }

        public bool? notifylonganswertime { get; set; }
        public long? longanswertime { get; set; }

        public bool? writelog { get; set; }
        public bool? writelogforeachhost { get; set; }

        public int? agentmeasurestime { get; set; }
        public bool? notifycpuoverload { get; set; }
        public int? notifycpuoverloadpercent { get; set; }
        public bool? notifylowfreemem { get; set; }
        public int? notifylowfreememmegabytes { get; set; }
        public bool? notifylowdisksspace { get; set; }
        public int? notifylowdisksspacemegabytes { get; set; }
        public bool? notifydisksoverload { get; set; }
        public int? notifydisksoverloadpercent { get; set; }
        public bool? notifynetoverload { get; set; }
        public int? notifynetoverloadpercent { get; set; }

        public bool? notifyagentlost { get; set; }

        public bool? notifyhostrestarted { get; set; }
        public bool? notifyhostgoingreboot { get; set; }
        public bool? notifyrebootbyuser { get; set; }
        public bool? forwardcriticalevents { get; set; }
        public bool? forwarderrorevents { get; set; }

        public bool? notifyonmonday { get; set; }
        public bool? notifyontuesday { get; set; }
        public bool? notifyonwednesday { get; set; }
        public bool? notifyonthursday { get; set; }
        public bool? notifyonmfriday { get; set; }
        public bool? notifyonsaturday { get; set; }
        public bool? notifyonsunday { get; set; }

    }
    public class Settings
    {
        public Settings()
        {
            NotificationWeekDays = new List<KeyValuePair<string, bool?>>();
        }

        public bool Enable_Email_Notifications { get; set; }
        public bool Enable_TelegramBot_Notifications { get; set; }
        public bool Enable_SMS_Notifications { get; set; }
        public bool Enable_Push_Notifications { get; set; }
        public int Hosts_Query_Interval_seconds { get; set; }
        public int ICMP_TimeOut_miliseconds { get; set; }
        public int HTTP_TimeOut_miliseconds { get; set; }
        public int Number_Of_Fails_Before_Send_Notification { get; set; }
        public bool Write_Hosts_Log { get; set; }
        public bool Write_Log_For_Each_Host { get; set; }
        public int Hosts_Visual_Type { get; set; }
        public int Monitor_Picture_Type { get; set; }

        public bool Enable_Query_ControlHost { get; set; }
        public string ControlHost_URI { get; set; }
        public bool Enable_ControlHost_Notifications { get; set; }


        public int Agent_Measure_Time_Before_Sent_Notifications_minutes { get; set; }
        public bool Agent_Notify_if_CPU_is_Overload { get; set; }
        public int Agent_CPU_Overload_Percent { get; set; }
        public bool Agent_Notify_if_LowFreeMem { get; set; }
        public int Agent_LowFreeMem_Megabytes { get; set; }
        public bool Agent_Notify_LowDisksFreeSpace { get; set; }
        public int Agent_LowDisksFreeSpace_Megabytes { get; set; }
        public bool Agent_Notify_if_Disks_Overload { get; set; }
        public int Agent_DisksOverload_Percent { get; set; }
        public bool Agent_Notify_if_NetAdapters_Overload { get; set; }
        public int Agent_NetAdaptersOverload_Percent { get; set; }

        public bool Agent_Notify_AgentConnectionLost { get; set; }

        public bool Notify_if_AnswerTime_IsLong { get; set; }
        public long LongAnswerTime_miliseconds { get; set; }

        public bool EventLOG_Notify_if_ComputerRestarted { get; set; }
        public bool EventLOG_Notify_if_ComputerGoingReboot { get; set; }
        public bool EventLOG_Notify_if_ComputerRebootedByUser { get; set; }
        public bool EventLOG_ForwardCriticalEvents { get; set; }
        public bool EventLOG_ForwardErrorEvents { get; set; }

        public List<KeyValuePair<string, bool?>> NotificationWeekDays;


        //This options used only for getting from Options View
        public bool NotifyOnMonday { get { var myitem = NotificationWeekDays.Where(x => x.Key == "Monday").FirstOrDefault().Value; return myitem.HasValue ? myitem.Value : false; } }
        public bool NotifyOnTuesday { get { var myitem = NotificationWeekDays.Where(x => x.Key == "Tuesday").FirstOrDefault().Value; return myitem.HasValue ? myitem.Value : false; } }
        public bool NotifyOnWednesday { get { var myitem = NotificationWeekDays.Where(x => x.Key == "Wednesday").FirstOrDefault().Value; return myitem.HasValue ? myitem.Value : false; } }
        public bool NotifyOnThursday { get { var myitem = NotificationWeekDays.Where(x => x.Key == "Thursday").FirstOrDefault().Value; return myitem.HasValue ? myitem.Value : false; } }
        public bool NotifyOnFriday { get { var myitem = NotificationWeekDays.Where(x => x.Key == "Friday").FirstOrDefault().Value; return myitem.HasValue ? myitem.Value : false; } }
        public bool NotifyOnSaturday { get { var myitem = NotificationWeekDays.Where(x => x.Key == "Saturday").FirstOrDefault().Value; return myitem.HasValue ? myitem.Value : false; } }
        public bool NotifyOnSunday { get { var myitem = NotificationWeekDays.Where(x => x.Key == "Sunday").FirstOrDefault().Value; return myitem.HasValue ? myitem.Value : false; } }

    }
}