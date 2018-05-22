using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IntegrationSamples.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult MyCloud()
        {            
            return View();
        }

        public ActionResult Social()
        {
            ViewBag.Message = "Your Social page.";

            return View();
        }
    }
}