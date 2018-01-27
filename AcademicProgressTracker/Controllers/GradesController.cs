using System;
using System.Collections;
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
        private readonly ApplicationDbContext _context;

        public GradesController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Grades
        public ActionResult Index()
        {
            //Populate view model
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var userModulesId = _context.UserModules.Where(x => x.UserId == userId).Select(x => x.ModuleId).ToList();
            var moduleList = _context.Module.Where(x => userModulesId.Contains(x.Id)).OrderBy(x => x.Name).ToList();

            var viewModel = new GradesViewModel
            {
                ModuleList = moduleList,
            };
            return View(viewModel);
        }

        //GET: Grades/Add/3?success=False
        public ActionResult Add(int id, bool success)
        {
            //Variables to populate viewmodel for grade add page
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var module = _context.Module.SingleOrDefault(x => x.Id == id);
            var courseworkList = _context.Coursework.Where(x => x.ModuleId == module.Id).ToList();
            var userResultsList = _context.UserResults.Where(x => x.UserId == userId
                                                       && x.Coursework.ModuleId == module.Id)
                                                       .ToList();

            if (module == null)
            {
                return HttpNotFound();
            }

            var viewModel = new GradesAddViewModel
            {
                Module = module,
                CourseworkList = courseworkList,
                Success = success,
            };

            //Prevents issue on view render if userResult count = 0
            if (userResultsList.Any())
            {
                viewModel.UserResults = userResultsList;
            }

            return View(viewModel);
        }

        // POST: Grades/AddGrades
        [HttpPost]
        public ActionResult AddGrades(GradesAddViewModel gradesAddViewModel)
        {
            //Model fails to populate properly - Variable populate it again. Used for validation fail
            var courseworkContext = _context.Coursework;
            var courseWorkId = gradesAddViewModel.UserResults.Select(x => x.CourseworkId).First();
            var module = courseworkContext.Where(x => x.Id == courseWorkId).Select(y => y.Module).First();

            if (!ModelState.IsValid)
            {
                //Work around for model binding failure
                var viewModel = new GradesAddViewModel
                {
                    CourseworkList = courseworkContext.Where(x => x.ModuleId == module.Id).ToList(),
                    Module = module,
                };

                return View("Add", viewModel);
            }

            //Variables to update db with new userResults
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var userResultContext = _context.UserResults;
            IList<UserResults> userResultsToRemove = new List<UserResults>();

            //Create new userResult for each table entry
            foreach (var userResults in gradesAddViewModel.UserResults)
            {
                var userResult = new UserResults
                {
                    UserId = userId,
                    CourseworkId = userResults.CourseworkId,
                    Mark = userResults.Mark,
                    AddedDateTime = System.DateTime.Now
                };

                var resultInDb = userResultContext.FirstOrDefault(x => x.UserId == userId && x.CourseworkId == userResult.CourseworkId);

                //Check if entry exists for that coursework already
                if (resultInDb != null)
                {
                    //Don't update datetime is mark is unchanged
                    if (userResult.Mark == resultInDb.Mark)
                    {
                        userResult.AddedDateTime = resultInDb.AddedDateTime;
                    }

                    //Remove userResult from db
                    userResultsToRemove.Add(resultInDb);
                    userResultContext.RemoveRange(userResultsToRemove);
                }

                //Add new user result
                userResultContext.Add(userResult);
            }

            _context.SaveChanges();

            return RedirectToAction("Add", new { id = module.Id, success = true });
        }
    }
}