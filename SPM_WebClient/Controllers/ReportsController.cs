using Newtonsoft.Json;
using SPM_WebClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPM_WebClient.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult Index(DateTime? date_from = null, DateTime? date_to = null, string answer_time_sign = null, string answer_time = null, string hostnames = null, string failed_only = null, string auto_scaling = null, string scaling_index = null)
        {

            #region Params handling

            DateTime load_date_from = date_from ?? DateTime.Now;
            DateTime load_date_to = date_to ?? DateTime.Now;

            Operators load_answer_time_sign = Operators.None;
            if (!string.IsNullOrEmpty(answer_time_sign))
            {
                switch (answer_time_sign)
                {
                    case ">":
                        load_answer_time_sign = Operators.Greather;
                        break;
                    case ">=":
                        load_answer_time_sign = Operators.Greather_Equals;
                        break;
                    case "<":
                        load_answer_time_sign = Operators.Less;
                        break;
                    case "<=":
                        load_answer_time_sign = Operators.Greather_Equals;
                        break;
                    case "=":
                        load_answer_time_sign = Operators.Equals;
                        break;
                    case "<>":
                        load_answer_time_sign = Operators.Not_Equals;
                        break;
                }
            }


           


            int load_answer_time = 0;
            if (!string.IsNullOrEmpty(answer_time))
            {
                int.TryParse(answer_time, out load_answer_time);
            }

            string[] load_hostnames = new string[0];
            if (!string.IsNullOrEmpty(hostnames))
            {
                load_hostnames = hostnames.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            }

            bool load_failed_only = false;
            if (!string.IsNullOrEmpty(failed_only))
            { load_failed_only = failed_only.ToLower() == "on"; }

            bool load_auto_scaling = false;
            if (!string.IsNullOrEmpty(auto_scaling))
            { load_auto_scaling = auto_scaling.ToLower() == "on"; }
            

            int load_scaling_index = 0;
            if (!string.IsNullOrEmpty(scaling_index))
            {
                int.TryParse(scaling_index, out load_scaling_index);
            }

            #endregion
            ReportsViewModel reportsViewModel;
            List<ReportHost> reportHosts = new List<ReportHost>();
           
            Spm_Api_Processor spm_api_processor = new Spm_Api_Processor(App_Globals.Url, App_Globals.ApiKey);
            try
            {
                reportHosts = spm_api_processor.GetReportHosts(
                    load_date_from, load_date_to, load_answer_time_sign, load_answer_time, load_hostnames, load_failed_only, load_auto_scaling, load_scaling_index);

                reportsViewModel = new ReportsViewModel(reportHosts) { ApiIsAvailable = true };
            }
            catch (Exception ex) 
            {
                reportsViewModel = new ReportsViewModel(reportHosts) { ApiIsAvailable = false , ConnectionErrorHeader = "You have API connection error. Check API configuration file (Config\\config.cfg)." + ex.Message };             
            }
           
            return View(reportsViewModel);
        }            
    }
}