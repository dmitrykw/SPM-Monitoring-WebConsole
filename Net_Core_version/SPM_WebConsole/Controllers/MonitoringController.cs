using Microsoft.AspNetCore.Mvc;
using SPM_WebConsole.Models;
using SPM_WebConsole.Models.ViewModels.Monitoring;
using Host = SPM_WebConsole.Models.Host;

namespace SPM_WebConsole.Controllers
{
    // [Authorize]
    public class MonitoringController : Controller
    {
        // GET: Monitoring

        public ActionResult Index(string group_filter = "All Hosts", string show_filter = null)
        {
            #region Custom for one client, who wants to set Showing Level by URL
            //If show_filter is set - we call SetCookie() method,
            //who Set the cookie and call again Index() method without show_filter argument.
            if (show_filter != null)
            {
                switch (show_filter.ToLower())
                {
                    case "set_show_all":
                        return SetCookie(group_filter, "ShowHostsFilterLevel", "0");

                    case "set_show_failedandwarning":
                        return SetCookie(group_filter, "ShowHostsFilterLevel", "1");

                    case "set_show_failedonly":
                        return SetCookie(group_filter, "ShowHostsFilterLevel", "2");
                }

            }
            #endregion

            var cookieReq = Request.Cookies["ShowHostsFilterLevel"];
            int show_hosts_filter_level = 0;
            if (cookieReq != null)
            {
                switch (cookieReq)
                {
                    case "1":
                        show_hosts_filter_level = 1;
                        break;
                    case "2":
                        show_hosts_filter_level = 2;
                        break;
                    default:
                        show_hosts_filter_level = 0;
                        break;
                }
            }


            MonitoringViewModel viewModel = new MonitoringViewModel(group_filter, show_hosts_filter_level);

            return GetView(viewModel);
        }


        public ActionResult Search(string search_filter)
        {
            if (search_filter != "")
            {
                MonitoringViewModel viewModel = new MonitoringViewModel(search_filter, true);
                return GetView(viewModel);
            }
            else
            {
                MonitoringViewModel viewModel = new MonitoringViewModel();
                return GetView(viewModel);
            }
        }


        private ActionResult GetView(MonitoringViewModel viewModel)
        {

            if (viewModel.Hosts.Count > 0)
            {
                switch (viewModel.Hosts.FirstOrDefault().HostVisualType)
                {
                    case "SmallMonitor":
                        return View("SmallMonitorView", viewModel);
                    case "String":
                        return View("StringView", viewModel);

                    default:
                        return View("Index", viewModel);
                }
            }
            else { return View("Index", viewModel); }
        }


        public ActionResult Details(int id)
        {
            MonitoringDetailsViewModel viewModel = new MonitoringDetailsViewModel(id);

            Host myhost = viewModel.Hosts.FirstOrDefault(x => x.ID == id);
            if (myhost != null)
                return PartialView(myhost);
            return NotFound("Sorry, the host you're looking for doesn't exist.");
        }

        public ActionResult HostLog(int id)
        {
            MonitoringHostLogViewModel viewModel = new MonitoringHostLogViewModel(id);
            KeyValuePair<int, string> hostlog = new KeyValuePair<int, string>();
            if (viewModel.HostsLogs.Count() > 0)
            {
                hostlog = viewModel.HostsLogs.FirstOrDefault(x => x.Key == id);
                return View(hostlog);
            }
            else { return View(hostlog); }

        }


        [HttpPost]
        public ActionResult SetCookie(string selectedgroupname, string cookie_name, string cookie_value)
        {
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(7);
            cookieOptions.SameSite = SameSiteMode.Lax;                        
            Response.Cookies.Append(cookie_name, cookie_value, cookieOptions);

            return RedirectToAction("Index", new { group_filter = selectedgroupname });
        }

        [HttpPost]
        public ActionResult Index(UpdateHostObj model)
        {
            if (model != null)
            {
                try
                {
                    string curr_hostid = model.hostid;
                    int.TryParse(curr_hostid, out int curr_hostid_int);


                    MonitoringViewModel viewModel = new MonitoringViewModel(curr_hostid_int);


                    viewModel.SendHostUpdateAPI(model);


                    return Json(new { status = 1, message = "Update Host Data Success" });
                }
                catch (Exception ex) { return Json(new { status = 0, message = "Failed Update Host Data. Exception: " + ex.Message }); }

            }
            return Json(new { status = 0, message = "Failed Update Host Data. Post Object is null" });
        }

        [HttpPost]
        public ActionResult PostSortedIDs(UpdateHostSortingObj model)
        {
            if (model != null)
            {
                try
                {

                    MonitoringViewModel viewModel = new MonitoringViewModel();

                    viewModel.SendHostSortingUpdateAPI(model);


                    return Json(new { status = 1, message = "Update hosts sorting Success" });
                }
                catch (Exception ex) { return Json(new { status = 0, message = "Failed Update Host Sorting. Exception: " + ex.Message }); }

            }
            return Json(new { status = 0, message = "Failed Update Host Sorting. Post Object is null" });
        }

        [HttpPost]
        public ActionResult AddHost(string selectedgroupname, string hostname, string description, string hosttype)
        {
            string group_filter = selectedgroupname;
            try
            {
                MonitoringViewModel viewModel = new MonitoringViewModel(group_filter);
                viewModel.AddHostAPI(hostname, description, selectedgroupname, hosttype);
                System.Threading.Thread.Sleep(2200);
            }
            catch { }


            return RedirectToAction("Index", "Monitoring", new { group_filter = group_filter });
        }


        [HttpPost]
        public ActionResult RemoveHost(string selectedgroupname, string hostid)
        {
            string group_filter = selectedgroupname;
            try
            {
                MonitoringViewModel viewModel = new MonitoringViewModel(group_filter);




                int.TryParse(hostid, out int hostid_int);

                viewModel.RemoveHostAPI(hostid_int);
                System.Threading.Thread.Sleep(2200);
            }
            catch { }


            return RedirectToAction("Index", "Monitoring", new { group_filter = group_filter });

        }
    }
}
