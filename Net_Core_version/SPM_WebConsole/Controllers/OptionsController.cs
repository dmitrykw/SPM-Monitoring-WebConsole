using Microsoft.AspNetCore.Mvc;
using SPM_WebConsole.Models.ViewModels.Options;
using SPM_WebConsole.Models;
using Microsoft.Extensions.Hosting;
using SPM_WebConsole.Models.ViewModels.Monitoring;

namespace SPM_WebConsole.Controllers
{
    public class OptionsController : Controller
    {
        // GET: Options
        public async Task<ActionResult> Index()
        {
            return await Task.Run(() =>
            {
                var viewModel = new OptionsViewModel();
                return View(viewModel);
            });                       
        }



        [HttpPost]
        public async Task<ActionResult> Index(UpdateSettingsObj model)
        {
            if (model != null)
            {
                try
                {
                    return await Task.Run(() =>
                    {
                        var viewModel = new OptionsViewModel();

                        viewModel.SendOptionsUpdateAPI(model);

                        return Json(new { status = 1, message = "Update Host Data Success" });
                    });

                    
                }
                catch (Exception ex) { return Json(new { status = 0, message = "Failed Update Options Data. Exception: " + ex.Message }); }
            }
            return Json(new { status = 0, message = "Failed Update Options Data. Post Object is null" });
        }
    }
}
