using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web.DynamicData;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Mvc;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace AcademicProgressTracker.Controllers
{
    public class SetupController : Controller
    {
        private ApplicationDbContext _context;

        public SetupController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Setup
        public ActionResult Index()
        {
            var coursesList = _context.Course.ToList();
            var yearList = _context.Year.ToList();
            var modulePlaceholder = _context.Module.Where(x => x.Id == 100000).ToList();
            var viewModel = new SetupViewModel
            {
                Course = coursesList,
                Year = yearList,
                Module = modulePlaceholder
            };
            return View(viewModel);
        }

        public JsonResult GetModules(int courseId, int yearId, int optional, int[] chosenModulesIds)
        {
            var modules = _context.Module.Where(x => x.CourseId == courseId
                                          && x.YearId == yearId
                                          && x.Optional == optional)
                                          .OrderBy(x => x.Name)
                                          .ToList();

            if (modules == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (chosenModulesIds == null)
            {
                return Json(modules, JsonRequestBehavior.AllowGet);
            }

            var moduleIds = modules.Select(x => x.Id).ToList();

            foreach (int id in chosenModulesIds)
            {
                if (moduleIds.Contains(id))
                {
                    modules.RemoveAll(x => x.Id == id);
                }
            }

            return Json(modules, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCredits(int[] moduleId)
        {
            var moduleDb = _context.Module;
            var moduleList = moduleDb.Select(x => x.Id).ToList();
            var totalCredits = 0;
            foreach (var id in moduleId)
            {
                if (moduleList.Contains(id))
                {
                    var creditValue = moduleDb.Where(x => x.Id == id).Select(y => y.Credits).Single();
                    totalCredits += creditValue;
                }
            }

            return new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = totalCredits };
        }

        //parameter = view model? could help with other call
        [System.Web.Http.HttpPost]
        public ActionResult CreateUserModule(SetupViewModel viewModel)
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            foreach (var module in viewModel.ModuleId)
            {
                var userModule = new UserModules()
                {
                    UserId = userId,
                    ModuleId = module,
                };

                if (userId != 0 && module != 0)
                {
                    _context.UserModules.Add(userModule);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Setup");

        }
    }
}