using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace AcademicProgressTracker.Controllers
{
    public class GradesController : Controller
    {
        private ApplicationDbContext _context;

        public GradesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Grades
        public ActionResult Index()
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var modules = _context.Module;
            var userModulesId = _context.UserModules.Where(x => x.UserId == userId).Select(x => x.ModuleId).ToList();
            var moduleList = modules.Where(x => userModulesId.Contains(x.Id)).OrderBy(x => x.Name).ToList();

            var viewModel = new GradesViewModel
            {
                ModuleList = moduleList,
            };
            return View(viewModel);
        }

        public ActionResult Add(int id)
        {
            var module = _context.Module.SingleOrDefault(x => x.Id == id);

            if (module == null)
            {
                return HttpNotFound();
            }

            var viewModel = new GradesViewModel
            {
                Module = module,
            };
            return View(viewModel);
        }
    }
}