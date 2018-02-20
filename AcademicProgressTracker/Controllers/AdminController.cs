using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.ViewModels;

namespace AcademicProgressTracker.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Delete(AdminDeleteViewModel viewModel)
        {
            var courseList = _context.Course.Where(y => y.Deleted == 0).OrderBy(x => x.Name).ToList();
            viewModel.CourseList = courseList;
            if (viewModel.Success)
            {
                return View(viewModel);
            }
            viewModel.Success = false;

            return View(viewModel);
        }

        [System.Web.Http.HttpPost]
        public ActionResult DeleteCourse(AdminDeleteViewModel viewModel)
        {
            var courseContext = _context.Course.ToList();
            var courseList = courseContext.Where(y => y.Deleted == 0).OrderBy(x => x.Name).ToList();
            courseContext.First(x => x.Id == viewModel.CourseToDeleteId).Deleted = 1;
            _context.SaveChanges();

            //Re-populate ddl on postback 
            viewModel.CourseList = courseList;
            viewModel.Success = true;

            return RedirectToAction("Delete", "Admin", viewModel);
        }
    }
}