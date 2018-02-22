using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public ActionResult Add(AdminAddViewModel viewModel)
        {
            var courseTypeList = new List<string>();
            var courseDurationList = new List<int>();
            var yearList = _context.Year.ToList();

            courseTypeList.Add("BA"); courseTypeList.Add("BAS"); courseTypeList.Add("BEng"); courseTypeList.Add("BSc"); courseTypeList.Add("MEng"); courseTypeList.Add("MSc");
            courseDurationList.Add(1); courseDurationList.Add(2); courseDurationList.Add(3); courseDurationList.Add(4);

            viewModel.CourseType = courseTypeList;
            viewModel.CourseDuration = courseDurationList;
            viewModel.YearList = yearList;

            return View(viewModel);
        }

        public ActionResult AddCourse(AdminAddViewModel viewModel)
        {
            //Add Course to db
            var courseToAdd = new Course()
            {
                Name = viewModel.CourseName,
                Type = viewModel.CourseType.First(),
                Duration = viewModel.CourseDuration.First(),
                Deleted = 0
            };
            _context.Course.Add(courseToAdd);
            _context.SaveChanges();

            //Get correct modules and add to db
            var courseId = _context.Course.Max(x => x.Id);

            foreach (var module in viewModel.Modules)
            {
                if (module.Name != null)
                {
                    module.CourseId = courseId;
                    module.YearId = module.YearId;
                    _context.Module.Add(module);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("AddCoursework", "Admin", viewModel);
        }

        public ActionResult AddCoursework()
        {
            var viewModelList = new List<AdminAddCourseworkViewModel>();
            var newCourseId = _context.Course.Max(x => x.Id);
            var modules = _context.Module.Where(x => x.CourseId == newCourseId);
            var i = 0;

            foreach (var module in modules)
            {
                var viewModel = new AdminAddCourseworkViewModel
                {
                    Module = module,
                    Index = i,
                };

                i++;
                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        [System.Web.Http.HttpPost]
        public ActionResult AddCourseworkAdd(List<AdminAddCourseworkViewModel> viewModelList)
        {
            foreach (var module in viewModelList)
            {
                foreach (var coursework in module.Coursework)
                {
                    var newCoursework = new Coursework
                    {
                        Name = coursework.Name,
                        ModuleId = module.Module.Id,
                        Percentage = coursework.Percentage
                    };

                    _context.Coursework.Add(newCoursework);
                }
            }

            _context.SaveChanges();

            return RedirectToAction("AddCoursework", "Admin", viewModelList);
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