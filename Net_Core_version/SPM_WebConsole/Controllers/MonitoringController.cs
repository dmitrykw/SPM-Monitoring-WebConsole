using Microsoft.AspNetCore.Mvc;
using SPM_WebConsole.Models;
using SPM_WebConsole.Models.ViewModels.Monitoring;
using System.Reflection;
using Host = SPM_WebConsole.Models.Host;

namespace SPM_WebConsole.Controllers
{
    // [Authorize]
    public class MonitoringController : Controller
    {
        // GET: Monitoring

        public async Task<ActionResult> Index(string group_filter = "All Hosts", string show_filter = null)
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


            return GetView(await Task.Run(() =>
            {
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
                
                return new MonitoringViewModel(group_filter, show_hosts_filter_level);
            }));                            
        }


        public async Task<ActionResult> Search(string search_filter)
        {
            return GetView(await Task.Run(() =>
            {
                if (!string.IsNullOrEmpty(search_filter))
                {
                    return new MonitoringViewModel(search_filter, true);                    
                }
                else
                {
                    return new MonitoringViewModel();                   
                }
            }));            
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


        public async Task<ActionResult> Details(int id)
        {
            Host myhost = await Task.Run(() =>
            {
                var viewModel = new MonitoringDetailsViewModel(id);
                return viewModel.Hosts.FirstOrDefault(x => x.ID == id) ?? new Host() {ID = int.MaxValue };
            });

            
            if (myhost.ID != int.MaxValue) { return PartialView(myhost); }
                
            return NotFound("Sorry, the host you're looking for doesn't exist.");
        }

        public async Task<ActionResult> HostLog(int id)
        {
            return View(await Task.Run(() =>
            {
                var viewModel = new MonitoringHostLogViewModel(id);
                var hostlog = new KeyValuePair<int, string>();
                if (viewModel.HostsLogs.Count() > 0)
                {
                    hostlog = viewModel.HostsLogs.FirstOrDefault(x => x.Key == id);
                    return hostlog;
                }
                return hostlog;                
            }));            
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
        public async Task<ActionResult> Index(UpdateHostObj model)
        {
            if (model != null)
            {
                try
                {
                    return await Task.Run(() =>
                    {
                        string curr_hostid = model.hostid;
                        int.TryParse(curr_hostid, out int curr_hostid_int);

                        var viewModel = new MonitoringViewModel(curr_hostid_int);

                        viewModel.SendHostUpdateAPI(model);
                        return Json(new { status = 1, message = "Update Host Data Success" });
                    });                                      
                }
                catch (Exception ex) { return Json(new { status = 0, message = "Failed Update Host Data. Exception: " + ex.Message }); }

            }
            return Json(new { status = 0, message = "Failed Update Host Data. Post Object is null" });
        }

        [HttpPost]
        public async Task<ActionResult> PostSortedIDs(UpdateHostSortingObj model)
        {
            if (model != null)
            {
                try
                {
                    return await Task.Run(() =>
                    {
                        var viewModel = new MonitoringViewModel();

                        viewModel.SendHostSortingUpdateAPI(model);

                        return Json(new { status = 1, message = "Update hosts sorting Success" });
                    });
                    
                }
                catch (Exception ex) { return Json(new { status = 0, message = "Failed Update Host Sorting. Exception: " + ex.Message }); }

            }
            return Json(new { status = 0, message = "Failed Update Host Sorting. Post Object is null" });
        }

        [HttpPost]
        public async Task<ActionResult> AddHost(string selectedgroupname, string hostname, string description, string hosttype)
        {
            string group_filter = selectedgroupname;
            try
            {
                return await Task.Run(() =>
                {
                    var viewModel = new MonitoringViewModel(group_filter);
                    viewModel.AddHostAPI(hostname, description, selectedgroupname, hosttype);

                    System.Threading.Thread.Sleep(2200);
                    return RedirectToAction("Index", "Monitoring", new { group_filter });
                });
            }
            catch {
                return RedirectToAction("Index", "Monitoring", new { group_filter });
            }

            
        }


        [HttpPost]
        public async Task<ActionResult> RemoveHost(string selectedgroupname, string hostid)
        {
            string group_filter = selectedgroupname;
            try
            {
                return await Task.Run(() =>
                {
                    var viewModel = new MonitoringViewModel(group_filter);

                    int.TryParse(hostid, out int hostid_int);

                    viewModel.RemoveHostAPI(hostid_int);

                    System.Threading.Thread.Sleep(10000);
                    return RedirectToAction("Index", "Monitoring", new { group_filter });
                });                
            }
            catch {
                return RedirectToAction("Index", "Monitoring", new { group_filter });
            }           
        }
    }
}
