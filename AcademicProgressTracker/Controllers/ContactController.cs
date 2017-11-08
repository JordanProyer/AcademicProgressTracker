using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AcademicProgressTracker.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}