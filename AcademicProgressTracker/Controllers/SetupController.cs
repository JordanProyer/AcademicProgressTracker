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
        private readonly ApplicationDbContext _context;

        public SetupController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Setup
        public ActionResult Index(SetupViewModel viewModel)
        {
            var coursesList = _context.Course.Where(y => y.Deleted == 0).OrderBy(x => x.Name).ToList();
            var yearList = _context.Year.ToList();
            var modulePlaceholder = _context.Module.Where(x => x.Id == 100000).ToList();
            viewModel.Course = coursesList;
            viewModel.Year = yearList;
            viewModel.Module = modulePlaceholder;

            viewModel.Success = false;

            return View(viewModel);
        }

        public JsonResult GetModules(int courseId, int yearId, int optional, int[] chosenModulesIds)
        {
            bool option = optional == 1;
            var modules = _context.Module.Where(x => x.CourseId == courseId
                                          && x.YearId == yearId
                                          && x.Optional == option)
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

        public JsonResult GetCompulsoryCredits(int courseId, int yearId, int optional)
        {
            bool option = optional == 1;
            var compulsoryCredits = _context.Module.Where(x => x.CourseId == courseId 
                                                        && x.YearId == yearId
                                                        && x.Optional == option).Select(y => y.Credits).Sum();

            return Json(compulsoryCredits, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCredits(int chosenModulesIds)
        {
            var moduleDb = _context.Module;
            var moduleList = moduleDb.Select(x => x.Id).ToList();
            var creditValue = 0;
            
                if (moduleList.Contains(chosenModulesIds))
                {
                     creditValue = moduleDb.Where(x => x.Id == chosenModulesIds).Select(y => y.Credits).Single();
                }

            return Json(creditValue, JsonRequestBehavior.AllowGet);
        }

        //parameter = view model? could help with other call
        [System.Web.Http.HttpPost]
        public ActionResult CreateUserModule(SetupViewModel viewModel)
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var compulsoryModules = _context.Module.Where(x => x.CourseId == viewModel.CourseId
                                                               && x.YearId == viewModel.YearId
                                                               && x.Optional == false).Select(x => x.Id);
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
                }
            }

            foreach (var module in compulsoryModules)
            {
                var userModule = new UserModules()
                {
                    UserId = userId,
                    ModuleId = module,
                };

                if (userId != 0 && module != 0)
                {
                    _context.UserModules.Add(userModule);
                }
            }

            _context.SaveChanges();

            viewModel.Success = true;
            return RedirectToAction("Index", "Setup", viewModel);

        }
    }
}