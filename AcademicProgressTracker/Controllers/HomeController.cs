using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.Models.Graphs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AcademicProgressTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IdentityDbContext _identityContext;

        public HomeController()
        {
            _context = new ApplicationDbContext();
            _identityContext = new IdentityDbContext();
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCompletedCourseworks()
        {   
            var resultsList = new List<CourseworkQuantity>();
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var userModules = _context.UserModules.Where(x => x.UserId == userId).Select(x => x.ModuleId).ToList();
            var numberOfCourseworks = _context.Coursework.Count(x => userModules.Contains(x.ModuleId));
            var numberofCompletedCourseworks = _context.UserResults.Count(x => x.UserId == userId && x.Mark != null);

            var graphObject = new CourseworkQuantity
            {
                Label = "Number of Courseworks Completed",
                CourseworkNumber = numberofCompletedCourseworks
            };

            resultsList.Add(graphObject);

            graphObject = new CourseworkQuantity
            {
                Label = "Number of Courseworks Remaining",
                CourseworkNumber = numberOfCourseworks - numberofCompletedCourseworks
            };

            resultsList.Add(graphObject);

            return Json(resultsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompletedModules()
        {
            var resultsList = new List<CourseworkQuantity>();
            var notCompletedModuleIdList = new List<int>();
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var userModules = _context.UserModules.Where(x => x.UserId == userId).Select(x => x.ModuleId).ToList();
            var completedCourseworks = _context.UserResults.Where(x => x.UserId == userId && x.Mark != null).Select(x => x.Coursework).ToList();
            var courseworksGroupedbyModule = _context.Coursework.Where(x => userModules.Contains(x.ModuleId)).GroupBy(y => y.ModuleId).ToList();

            foreach (var module in courseworksGroupedbyModule)
            {
                foreach (var coursework in module)
                {
                    if (!completedCourseworks.Contains(coursework))
                    {
                        var moduleId = module.Key;
                        notCompletedModuleIdList.Add(moduleId);
                    }
                }
            }

            var distinctList = notCompletedModuleIdList.Distinct();
            var completedModules = userModules.Count - distinctList.Count();
            var graphObject = new CourseworkQuantity
            {
                Label = "Number of Modules Completed",
                CourseworkNumber = completedModules
            };

            resultsList.Add(graphObject);

            graphObject = new CourseworkQuantity
            {
                Label = "Number of Modules Remaining",
                CourseworkNumber = userModules.Count - completedModules
            };

            resultsList.Add(graphObject);

            return Json(resultsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAverageMark()
        {
            var resultsList = new List<CourseworkQuantity>();
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var averageMark = Math.Round(Convert.ToDouble(_context.UserResults.Where(x => x.UserId == userId).Average(x => x.Mark)), 2);

            var graphObject = new CourseworkQuantity
            {
                Label = "Average Mark (%)",
                CourseworkNumber = averageMark,
            };

            resultsList.Add(graphObject);

            graphObject = new CourseworkQuantity
            {
                Label = "Distance from 100%",
                CourseworkNumber = Math.Round(100 - averageMark,2),
            };

            resultsList.Add(graphObject);

            return Json(resultsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTotalNumberOfUsers()
        {
            var resultsList = new List<CourseworkQuantity>();
            var totalusers = _identityContext.Users.Count();
            var graphObject = new CourseworkQuantity
            {
                Label = "Total Number of Users",
                CourseworkNumber = totalusers
            };

            resultsList.Add(graphObject);

            return Json(resultsList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTotalNumberOfResults()
        {
            var resultsList = new List<CourseworkQuantity>();
            var totalNumberOfResults = _context.UserResults.Count();
            var graphObject = new CourseworkQuantity
            {
                Label = "Total Number of Results Entered",
                CourseworkNumber = totalNumberOfResults,
            };

            resultsList.Add(graphObject);

            return Json(resultsList, JsonRequestBehavior.AllowGet);
        }

    }
}