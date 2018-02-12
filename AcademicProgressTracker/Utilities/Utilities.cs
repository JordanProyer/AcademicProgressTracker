using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using AcademicProgressTracker.Models;
using AcademicProgressTracker.Models.Graphs;
using Microsoft.AspNet.Identity;

namespace AcademicProgressTracker.Utilities
{
    public class Utilities
    {
        private readonly ApplicationDbContext _context;

        public Utilities()
        {
            _context = new ApplicationDbContext();

        }
        public List<KnnResult> KNeareastNeighbour(int userId, int moduleId, int k)
        {
            //Set up variables
            var userResult = _context.UserResults.Where(x => x.UserId == userId && x.Coursework.ModuleId == moduleId).ToList();
            var allUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId != userId).ToList();
            decimal? moduleMark = 0;
            int individualCourseworkCount = 0;
            int numOfCourseworks = userResult.Count;
            int markPercentRange = 10;
            int numOfValidResults = 0;
            bool enoughResults = false;

            if (k == 0 || k > 5)
            {
                k = 5;
            }

            //Remove all results for users who have not fully completed module or not within specified range
            //If not enough results left, double search range until sufficient amount found
            do
            {
                foreach (var result in allUserResults.ToList())
                {
                    var comparableUserResult = userResult.First(x => x.CourseworkId == result.CourseworkId);

                    if (result.Mark == null || !IsResultInRange(comparableUserResult, result, markPercentRange))
                    {
                        allUserResults.RemoveAll(x => x.UserId == result.UserId);
                    }
                }

                //Check for enough users with a complete result list
                foreach (var group in allUserResults.GroupBy(x => x.UserId))
                {
                    var count = group.Count();
                    if (count == numOfCourseworks)
                    {
                        numOfValidResults++;
                    }
                }

                //If there is not enough results, double the percent range
                if (numOfValidResults < k)
                {
                    numOfValidResults = 0;
                    allUserResults.Clear();
                    allUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId != userId).ToList();
                    markPercentRange += 10;

                    //If no results are in a reasonable range, return 0
                    if (markPercentRange > 200)
                    {
                        return null;
                    }
                }

                else
                {
                    enoughResults = true;
                }

            } while (enoughResults == false);

            //Set up lists
            List<KnnResult> knnResultList = new List<KnnResult>();
            List<double> values = new List<double>();

            //Group results by users to be iterated through
            foreach (var group in allUserResults.GroupBy(x => x.UserId))
            {
                //Iterated through for each coursework
                foreach (var coursework in allUserResults.Where(x => x.UserId == group.Key))
                {
                    //Get mark for current user and square distance between it and existing mark (for one coursework)
                    var userResultMark = userResult.FirstOrDefault(x => x.Mark != null && x.CourseworkId == coursework.CourseworkId);
                    if (userResultMark != null)
                    {
                        var userMark = Convert.ToDouble(userResultMark.Mark);
                        var distance = Math.Pow(userMark - Convert.ToDouble(coursework.Mark), 2);
                        values.Add(distance);

                        moduleMark += coursework.Mark;
                        individualCourseworkCount++;
                    }
                }

                //Calculate total distance between all marks for two users
                var totalDistance = KNNFactor(values);
                var knnResult = new KnnResult()
                {
                    UserId = group.Key,
                    Distance = totalDistance,
                    AverageModuleMark = Math.Round((Convert.ToDouble(moduleMark) / individualCourseworkCount),2)
                };

                knnResultList.Add(knnResult);

                //Clear distance list for next iteration
                values.Clear();
                moduleMark = 0;
                individualCourseworkCount = 0;
            }

            //Order all results by distance asc and take first k results
            var orderedKnnResultList = knnResultList.OrderBy(x => x.Distance).Take(k).ToList();
            SetLabelName(orderedKnnResultList);
            return orderedKnnResultList;
        }

        public double GetKnnResultNumber(int userId, int moduleId)
        {
            var allUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId != userId).ToList();
            var knnOrderedResultList = KNeareastNeighbour(userId, moduleId, 3);

            //Get coursework count for chosen k users
            var relevantUserResults = allUserResults.Where(x => knnOrderedResultList.Select(y => y.UserId).Contains(x.UserId)).ToList();
            var courseworkCount = relevantUserResults.Count;

            //Calculate average final mark for chosen k users
            if (courseworkCount > 0)
            {
                var totalMark = relevantUserResults.Select(x => x.Mark).Sum();
                var averageMark = totalMark / courseworkCount;
                return Convert.ToDouble(averageMark);
            }

            return 0;
        }

        public String GetKnnResultText(double result)
        {
            var roundResult = Math.Round(result, 2);
            return _context.Classification.First(x => x.UpperBound >= roundResult && x.LowerBound <= roundResult).Name;
        }

        private bool IsResultInRange(UserResults userResult, UserResults existingUserResult, decimal range)
        {
            decimal upperRange = 1 + range / 100;
            decimal lowerRange = 1 - range / 100;

            if (userResult.Mark * upperRange < existingUserResult.Mark ||
                userResult.Mark * lowerRange > existingUserResult.Mark)
            {
                return false;
            }

            return true;
        }

        //Square roots the sum of squared distances to find total distance
        private double KNNFactor(List<double> numberList)
        {
            var total = numberList.Sum();
            var distance = Math.Sqrt(total);
            return distance;
        }

        private void SetLabelName(List<KnnResult> resultList)
        {
            int positionInList = 0;

            foreach (var result in resultList)
            {
                String label;
                switch (positionInList)
                {
                    case 0: label = "Nearest Neighbour"; break;
                    case 1: label = "Second Nearest Neighbour"; break;
                    case 2: label = "Third Nearest Neighbour"; break;
                    case 3: label = "Fourth Neighbour"; break;
                    case 4: label = "Fifth Neighbour"; break;
                    case 5: label = "Sixth Neighbour"; break;
                    case 6: label = "Seventh Neighbour"; break;
                    case 7: label = "Eighth Neighbour"; break;
                    case 8: label = "Nineth Neighbour"; break;
                    case 9: label = "Tenth Neighbour"; break;
                    default: label = "A Near Neighbour"; break;
                }

                result.Label = label;
                positionInList++;
            }
        }

        public MaximumWeightedGrade CalculateMaximumPercentage(List<UserResults> userResultsList)
        {
            List<WeightedResult> weightedGradesList = new List<WeightedResult>();

            foreach (var result in userResultsList)
            {
                var weightedResult = new WeightedResult()
                {
                    Mark = WeightedMark(Convert.ToDouble(result.Mark), result.Coursework.Percentage),
                    Percentage = result.Coursework.Percentage
                };

                weightedGradesList.Add(weightedResult);
            }

            var currentPercentage = weightedGradesList.Where(x => true).Select(x => x.Percentage).Sum();
            var remainingPercentage = 100 - currentPercentage;

            var currentWeightedMark = weightedGradesList.Where(x => true).Select(x => x.Mark).Sum();
            var maximumWeightedMark = currentWeightedMark + remainingPercentage;

            var maximumWeightedGrade = new MaximumWeightedGrade
            {
                CurrentWeightedMark = Convert.ToDecimal(currentWeightedMark),
                MaximumWeightedMark = maximumWeightedMark
            };

            return maximumWeightedGrade;
        }


        private double WeightedMark(double moduleMark, double percentage)
        {
            var weighting = percentage / 100;
            var weightedMark = moduleMark * weighting;
            return weightedMark;
        }

        public decimal? WeightedMark(UserResults userResult)
        {
            var mark = userResult.Mark;
            var courseworkId = userResult.CourseworkId;
            decimal percentage = _context.Coursework.First(x => x.Id == courseworkId).Percentage;
            var weighting = percentage / 100;
            var weightedMark = mark * weighting;

            return weightedMark;
        }


    }
}