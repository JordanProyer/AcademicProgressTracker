using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.ViewModels;
using Microsoft.AspNet.Identity;

namespace AcademicProgressTracker.Controllers
{
    public class OverviewController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OverviewController()
        {
            _context = new ApplicationDbContext();
        }

        //OverviewViewModel is for entire panel
        //UserModuleResult is for list of modules with result and classification
        public ActionResult Index()
        {
            //Individual lists to get around being unable to clear list for each new year
            List<OverviewViewModel> overviewViewModelList = new List<OverviewViewModel>();
            List<UserModuleResult> currentUserModuleResultList = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear1List = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear2List = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear3List = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear4List = new List<UserModuleResult>();
            var util = new Utilities.Utilities();
            var userId = Convert.ToInt32(User.Identity.GetUserId());

            var userYears = _context.UserModules.Where(x => x.UserId == userId).Select(y => y.Module.Year).GroupBy(z => z.Id).Select(grp => grp.FirstOrDefault()).ToList();
            var userModules = _context.UserModules.Where(x => x.UserId == userId).Select(y => y.Module).ToList();
            var userResultContext = _context.UserResults.Include(x => x.Coursework.Module);
            var userResultsGrouped = userResultContext.Where(x => x.UserId == userId).GroupBy(y => y.Coursework.Module).ToList();
            var classificationList = _context.Classification;

            //Loop over each coursework grouped ny module for each year
            foreach (var year in userYears)
            {
                foreach (var userResult in userResultsGrouped.Where(x => x.Key.Year.Id == year.Id))
                {
                    double weightedMark = util.WeightedMark(userResult.ToList());

                    var userModuleResult = new UserModuleResult
                    {
                        ModuleName = userResult.Key.Name,
                        ModuleId = userResult.Key.Id,
                        Mark = Math.Round(weightedMark, 2),
                        Classification = classificationList.First(x => x.LowerBound <= weightedMark && x.UpperBound >= weightedMark).Value,
                    };

                    if (year.Id == 1)
                    {
                        userModuleResultYear1List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear1List;
                    }

                    else if (year.Id == 2)
                    {
                        userModuleResultYear2List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear2List;
                    }

                    else if (year.Id == 3)
                    {
                        userModuleResultYear3List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear3List;
                    }

                    else if (year.Id == 4)
                    {
                        userModuleResultYear4List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear4List;
                    }

                }

                //Add in all modules user has not yet entered
                foreach (var module in userModules.Where(x => x.Year.Id == year.Id))
                {
                    if (!currentUserModuleResultList.Select(x => x.ModuleName).Contains(module.Name))
                    {
                        var nonUserModuleResult = new UserModuleResult
                        {
                            ModuleName = module.Name,
                            ModuleId = module.Id,
                            Mark = 0,
                            Classification = "Unavailable",
                        };

                        if (year.Id == 1)
                        {
                            userModuleResultYear1List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear1List;
                        }

                        else if (year.Id == 2)
                        {
                            userModuleResultYear2List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear2List;
                        }

                        else if (year.Id == 3)
                        {
                            userModuleResultYear3List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear3List;
                        }

                        else if (year.Id == 4)
                        {
                            userModuleResultYear4List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear4List;
                        }
                    }
                }

                currentUserModuleResultList.OrderBy(x => x.ModuleName);
                var weightedMarkbycredits = util.WeightedMarkForModule(currentUserModuleResultList);

                //Add total for each year
                var finalResult = new UserModuleResult()
                {
                    ModuleName = "Total",
                    Mark = weightedMarkbycredits,
                    Classification = classificationList.First(x => x.LowerBound <= weightedMarkbycredits && x.UpperBound >= weightedMarkbycredits).Value,
                };

                if (year.Id == 1)
                {
                    userModuleResultYear1List.OrderBy(x => x.ModuleName);
                    userModuleResultYear1List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear1List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }

                else if (year.Id == 2)
                {
                    userModuleResultYear2List.OrderBy(x => x.ModuleName);
                    userModuleResultYear2List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear2List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }

                else if (year.Id == 3)
                {
                    userModuleResultYear3List.OrderBy(x => x.ModuleName);
                    userModuleResultYear3List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear3List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }

                else if (year.Id == 4)
                {
                    userModuleResultYear4List.OrderBy(x => x.ModuleName);
                    userModuleResultYear4List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear4List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }
            }

            return View("Index", overviewViewModelList);
        }

        public ActionResult KnnIndex()
        {
            //Individual lists to get around being unable to clear list for each new year
            List<OverviewViewModel> overviewViewModelList = new List<OverviewViewModel>();
            List<UserModuleResult> currentUserModuleResultList = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear1List = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear2List = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear3List = new List<UserModuleResult>();
            List<UserModuleResult> userModuleResultYear4List = new List<UserModuleResult>();
            var util = new Utilities.Utilities();
            var userId = Convert.ToInt32(User.Identity.GetUserId());

            var userYears = _context.UserModules.Where(x => x.UserId == userId).Select(y => y.Module.Year).GroupBy(z => z.Id).Select(grp => grp.FirstOrDefault()).ToList();
            var userModules = _context.UserModules.Where(x => x.UserId == userId).Select(y => y.Module).ToList();
            var userResultContext = _context.UserResults.Include(x => x.Coursework.Module);
            var userResultsGrouped = userResultContext.Where(x => x.UserId == userId).GroupBy(y => y.Coursework.Module).ToList();
            var classificationList = _context.Classification;

            //Loop over each coursework grouped ny module for each year
            foreach (var year in userYears)
            {
                foreach (var userResult in userResultsGrouped.Where(x => x.Key.Year.Id == year.Id))
                {
                    double weightedMark = 0;
                    if (!userResultContext.Where(x => x.UserId == userId && x.Coursework.Module.Id == userResult.Key.Id).All(z => z.Mark != null))
                    {
                        weightedMark = util.GetKnnResultNumber(userId, userResult.Key.Id, 3);
                    }

                    else
                    {
                        weightedMark = util.WeightedMark(userResult.ToList());
                    }

                    var userModuleResult = new UserModuleResult
                    {
                        ModuleName = userResult.Key.Name,
                        ModuleId = userResult.Key.Id,
                        Mark = Math.Round(weightedMark, 2),
                        Classification = classificationList.First(x => x.LowerBound <= weightedMark && x.UpperBound >= weightedMark).Value,
                    };

                    if (year.Id == 1)
                    {
                        userModuleResultYear1List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear1List;
                    }

                    else if (year.Id == 2)
                    {
                        userModuleResultYear2List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear2List;
                    }

                    else if (year.Id == 3)
                    {
                        userModuleResultYear3List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear3List;
                    }

                    else if (year.Id == 4)
                    {
                        userModuleResultYear4List.Add(userModuleResult);
                        currentUserModuleResultList = userModuleResultYear4List;
                    }

                }

                //Add in all modules user has not yet entered
                foreach (var module in userModules.Where(x => x.Year.Id == year.Id))
                {
                    if (!currentUserModuleResultList.Select(x => x.ModuleName).Contains(module.Name))
                    {
                        var nonUserModuleResult = new UserModuleResult
                        {
                            ModuleName = module.Name,
                            ModuleId = module.Id,
                            Mark = 0,
                            Classification = "Unavailable",
                        };

                        if (year.Id == 1)
                        {
                            userModuleResultYear1List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear1List;
                        }

                        else if (year.Id == 2)
                        {
                            userModuleResultYear2List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear2List;
                        }

                        else if (year.Id == 3)
                        {
                            userModuleResultYear3List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear3List;
                        }

                        else if (year.Id == 4)
                        {
                            userModuleResultYear4List.Add(nonUserModuleResult);
                            currentUserModuleResultList = userModuleResultYear4List;
                        }
                    }
                }

                currentUserModuleResultList.OrderBy(x => x.ModuleName);
                var weightedMarkbycredits = util.WeightedMarkForModule(currentUserModuleResultList);

                //Add total for each year
                var finalResult = new UserModuleResult()
                {
                    ModuleName = "Total",
                    Mark = weightedMarkbycredits,
                    Classification = classificationList.First(x => x.LowerBound <= weightedMarkbycredits && x.UpperBound >= weightedMarkbycredits).Value,
                };

                if (year.Id == 1)
                {
                    userModuleResultYear1List.OrderBy(x => x.ModuleName);
                    userModuleResultYear1List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear1List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }

                else if (year.Id == 2)
                {
                    userModuleResultYear2List.OrderBy(x => x.ModuleName);
                    userModuleResultYear2List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear2List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }

                else if (year.Id == 3)
                {
                    userModuleResultYear3List.OrderBy(x => x.ModuleName);
                    userModuleResultYear3List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear3List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }

                else if (year.Id == 4)
                {
                    userModuleResultYear4List.OrderBy(x => x.ModuleName);
                    userModuleResultYear4List.Add(finalResult);

                    var overviewViewModel = new OverviewViewModel
                    {
                        YearName = year.Name,
                        ModuleResultList = userModuleResultYear4List
                    };

                    overviewViewModelList.Add(overviewViewModel);
                }
            }

            return View("KnnIndex", overviewViewModelList);
        }

        public ActionResult Redirect(bool showKnnResult)
        {
            if (showKnnResult)
            {
                return Redirect("/Overview/KnnIndex");
            }

            else
            {
                return Redirect("/Overview/Index");
            }
        }
    }
}