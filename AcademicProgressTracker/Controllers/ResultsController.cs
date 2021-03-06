﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.Models.Graphs;
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
            var userModuleList = _context.UserModules.Where(x => x.UserId == userId).Include(y => y.Module.Year).OrderByDescending(z => z.Module.Year.Id).ThenBy(z => z.Module.Name).ToList();
            var moduleList = _context.Module.ToList();
            var courseworkList = _context.Coursework.ToList();
            var userResultsList = _context.UserResults.Where(x => x.UserId == userId).ToList();
            var classificationList = _context.Classification.ToList();

            foreach (var userModule in userModuleList.ToList())
            {
                var viewModel = new ResultsViewModel
                {
                    ModuleName = moduleList.First(x => x.Id == userModule.ModuleId).Name,
                    YearName = moduleList.First(x => x.Id == userModule.ModuleId).Year.Name,
                    ModuleId = moduleList.First(x => x.Id == userModule.ModuleId).Id,
                    CompletedCoursework = userResultsList.Count(x => x.Coursework.ModuleId == userModule.ModuleId && x.Mark != null),
                    TotalCourseworks = courseworkList.Count(x => x.ModuleId == userModule.ModuleId),              
                };

                var averageMark = userResultsList.Where(x => x.Coursework.ModuleId == userModule.ModuleId).Average(y => y.Mark);
                if (averageMark != null)
                {
                    viewModel.AverageMark = decimal.Round((decimal) averageMark, 2, MidpointRounding.AwayFromZero);
                }

                var roundResult = Math.Round(Convert.ToDouble(viewModel.AverageMark), 2);
                var classification = classificationList.FirstOrDefault(x => x.LowerBound <= roundResult && x.UpperBound >= roundResult)?.Value;
                viewModel.PredictedClassification = classification ?? "Currently unavailable";
                viewModelList.Add(viewModel);
            }

            return View(viewModelList.ToList());
        }

        public ActionResult Details(int id)
        {
            var module = _context.Module.First(x => x.Id == id);
            var viewModel = new ResultsDetailedViewModel
            {
                Module = module,
            };

            return View(viewModel);
        }

        public JsonResult GetCourseworkGrades(int moduleId)
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var relevantUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId == userId).Include(y => y.Coursework).ToList();
            var courseworkGradesList = new List<CourseworkGrades>();

            foreach (var userResult in relevantUserResults)
            {
                var courseworkGrades = new CourseworkGrades
                {
                    Name = userResult.Coursework.Name,
                    Mark = userResult.Mark,                   
                };

                courseworkGradesList.Add(courseworkGrades);
            }
            
            return Json(courseworkGradesList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCourseworkGradesOverTime(int moduleId)
        {
            var util = new Utilities.Utilities();
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var relevantUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId == userId).Include(y => y.Coursework).OrderBy(z => z.AddedDateTime).ToList();
            var courseworkGradesOverTimeList = new List<CourseworkGradesOverTime>();
            decimal? totalWeightedMark = 0;

            foreach (var userResult in relevantUserResults)
            {
                var thisWeightedMark = util.WeightedMark(userResult);
                if (thisWeightedMark != null)
                {
                    totalWeightedMark += util.WeightedMark(userResult);
                }

                if (userResult.AddedDateTime != null)
                {
                    var courseworkGradesOverTime = new CourseworkGradesOverTime
                    {
                        Mark = userResult.Mark,
                        WeightedMark = totalWeightedMark,
                        AddedDateTime = DateTimeOffset.Parse(userResult.AddedDateTime.Value.ToLongDateString()).ToUnixTimeMilliseconds()
                    };

                    courseworkGradesOverTimeList.Add(courseworkGradesOverTime);
                }
            }

            return Json(courseworkGradesOverTimeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaximumWeightedGrade(int moduleId)
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var util = new Utilities.Utilities();
            var relevantUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId == userId && x.Mark != null).Include(y => y.Coursework).ToList();
            List<MaximumWeightedGrade> maxWeightedGradeList = new List<MaximumWeightedGrade>();

            var maxWeightedGrade = util.CalculateMaximumPercentage(relevantUserResults);
            maxWeightedGradeList.Add(maxWeightedGrade);

            return Json(maxWeightedGradeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetKnnResult(int moduleId, int kValue)
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var util = new Utilities.Utilities();
            var userResultContext = _context.UserResults.Include(x => x.Coursework);
            var knnResultList = util.KNeareastNeighbour(userId, moduleId, kValue).ToList();


            var userResultsForModule = userResultContext.Where(x => x.Coursework.ModuleId == moduleId && x.UserId == userId).ToList();
            var userResultforCompletedCw = userResultsForModule.Where(x => x.Mark != null).ToList();

            //Circle size stuff 
            var allUserResults = userResultContext.Where(x => x.Coursework.ModuleId == moduleId).ToList();
            var stdDev = util.StandardDeviation(allUserResults);
            var mean = util.MeanMark(allUserResults);
            var predictedModuleMark = util.GetKnnResultNumber(userId, moduleId, kValue);

            var markAfterXCourseworks = util.WeightedMark(userResultforCompletedCw);
            if (userResultsForModule.Count == userResultforCompletedCw.Count)
            {
                markAfterXCourseworks = predictedModuleMark;
            }

            var userKnnResult = new KnnResult
            {
                MarkAfterXCourseworks = markAfterXCourseworks,
                PredictedModuleMark = predictedModuleMark,
                Label = "Your Result",
                UserId = userId,
                CircleSize = util.ProbabilityDensity(stdDev, mean, predictedModuleMark)
            };

            knnResultList.Insert(0, userKnnResult);


            return Json(knnResultList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMarksToClassification(int moduleId)
        {
            var userId = Convert.ToInt32(User.Identity.GetUserId());
            var util = new Utilities.Utilities();
            var relevantUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId == userId && x.Mark != null).Include(y => y.Coursework).ToList();

            var marksToClassificationList = util.CalculateNeededMarks(relevantUserResults);
            return Json(marksToClassificationList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetProbabilityDensityFunction(int moduleId)
        {
            var resultList= _context.UserResults.Include(x => x.Coursework).Where(x => x.Coursework.ModuleId == moduleId).ToList();
            var util = new Utilities.Utilities();
            var result = util.ProbabilityDensity(resultList);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}