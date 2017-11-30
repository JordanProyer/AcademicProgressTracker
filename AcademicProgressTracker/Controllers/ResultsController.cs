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
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultsController()
        {
            _context = new ApplicationDbContext();
        }

        // GET: Results
        public ActionResult Index()
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var viewModelList = new List<ResultsViewModel>();
            var userModuleList = _context.UserModules.Where(x => x.UserId == userId).OrderBy(y => y.Module.Name);
            var moduleList = _context.Module;
            var courseworkList = _context.Coursework;
            var userResultsList = _context.UserResults.Where(x => x.UserId == userId);
            var classificationList = _context.Classification;

            foreach (var userModule in userModuleList)
            {
                var viewModel = new ResultsViewModel
                {
                    ModuleName = moduleList.First(x => x.Id == userModule.ModuleId).Name,
                    ModuleId = moduleList.First(x => x.Id == userModule.ModuleId).Id,
                    CompletedCoursework = userResultsList.Count(x => x.Coursework.ModuleId == userModule.ModuleId),
                    TotalCourseworks = courseworkList.Count(x => x.ModuleId == userModule.ModuleId),              
                };

                var averageMark = userResultsList.Where(x => x.Coursework.ModuleId == userModule.ModuleId).Average(y => y.Mark);
                if (averageMark != null)
                {
                    viewModel.AverageMark = decimal.Round((decimal) averageMark, 2, MidpointRounding.AwayFromZero);
                }

                var classification = classificationList.FirstOrDefault(x => x.LowerBound <= viewModel.AverageMark && x.UpperBound >= viewModel.AverageMark)?.Value;
                viewModel.PredictedClassification = classification ?? "Currently unavailable";
                viewModelList.Add(viewModel);
            }

            return View(viewModelList);
        }

        public ActionResult Details(int id)
        {
            var module = _context.Module.First(x => x.Id == id);
            var viewModel = new ResultsAddViewModel
            {
                Module = module
            };
            return View(viewModel);
        }
    }
}