using System;
using SPM_WebClient.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPM_WebClient.Controllers
{
    public class OptionsController : Controller
    {
        // GET: Options
        public ActionResult Index()
        {
            OptionsViewModel viewModel = new OptionsViewModel();                        
            return View(viewModel);
        }



        [HttpPost]
        public ActionResult Index(UpdateSettingsObj model)
        {
            if (model != null)
            {
                try
                {
                    OptionsViewModel viewModel = new OptionsViewModel();
                  
                    viewModel.SendOptionsUpdateAPI(model);
                
                    return Json(new { status = 1, message = "Update Host Data Success" });
                }
                catch (Exception ex) { return Json(new { status = 0, message = "Failed Update Options Data. Exception: " + ex.Message }); }
            }
            return Json(new { status = 0, message = "Failed Update Options Data. Post Object is null" });
        }
    }
}