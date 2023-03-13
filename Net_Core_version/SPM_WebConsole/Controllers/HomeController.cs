using Microsoft.AspNetCore.Mvc;
using SPM_WebConsole.Models;
using System.Diagnostics;

namespace SPM_WebConsole.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult About()
        {
            ViewData["Message"] = "SPM Monitoring System Web Console.";

            return View();
        }      
    }
}