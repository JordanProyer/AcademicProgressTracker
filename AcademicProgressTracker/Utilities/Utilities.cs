using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var everyUserResultForCourseworks = _context.UserResults.Where(x => x.Coursework.Module.Id == moduleId).Include(y => y.Coursework).ToList();
            var allUserResults = everyUserResultForCourseworks.Where(x => x.UserId != userId).ToList();
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

                    //If not enough results are in a reasonable range, continue with valid ones
                    if (markPercentRange > 200)
                    {
                        enoughResults = true;
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
                    }
                }

                //Gets weighted mark for each user based on modules our user has compler
                var userResultsForUsersCompletedCw = allUserResults.Where(x => userResult.Where(z => z.Mark != null).Select(y => y.CourseworkId).Contains(x.CourseworkId)).ToList();
                var weightedMark = WeightedMark(userResultsForUsersCompletedCw.Where(x => x.UserId == group.Key).ToList());

                //Calculate total distance between all marks for two users
                var totalDistance = KNNFactor(values);
                var knnResult = new KnnResult()
                {
                    UserId = group.Key,
                    Distance = Math.Round(totalDistance, 2),
                    PredictedModuleMark = WeightedMark(allUserResults.Where(x => x.UserId == group.Key).ToList()),
                    MarkAfterXCourseworks = weightedMark,
                };

                knnResultList.Add(knnResult);

                //Clear distance list for next iteration
                values.Clear();
            }

            //Order all results by distance asc and take first k results
            var orderedKnnResultList = knnResultList.OrderBy(x => x.Distance).Take(k).ToList();
            SetNeighbourLabelName(orderedKnnResultList);
            return orderedKnnResultList;
        }

        public double GetAverageMark(List<UserResults> userResultList)
        {
            var totalMark = Convert.ToDouble(userResultList.Sum(x => x.Mark));
            var numOfCw = userResultList.Count;

            return Math.Round(totalMark / numOfCw , 2);
        }

        public double GetKnnResultNumber(int userId, int moduleId, int kValue)
        {
            var allUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId != userId).ToList();
            var knnOrderedResultList = KNeareastNeighbour(userId, moduleId, kValue);

            //Get coursework count for chosen k users
            var relevantUserResults = allUserResults.Where(x => knnOrderedResultList.Select(y => y.UserId).Contains(x.UserId)).GroupBy(z => z.UserId).ToList();

            double theTotalMark = 0;
            List<double> numbersToAdd = new List<double>();

            foreach (var group in relevantUserResults)
            {
                foreach (var entry in group)
                {
                    theTotalMark += Convert.ToDouble(entry.Mark);
                }

                var cwCount = group.Count();
                var groupAverage = theTotalMark / cwCount;
                numbersToAdd.Add(groupAverage);
                theTotalMark = 0;
                cwCount = 0;

            }

            return Math.Round(numbersToAdd.Sum() / numbersToAdd.Count, 2);
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

        public List<ProbabilityDensity> ProbabilityDensity(List<UserResults> userResultList)
        {
            var stDev = StandardDeviation(userResultList);
            var mean = MeanMark(userResultList);

            var resultList = new List<ProbabilityDensity>();
            var firstValue = 1 / Math.Sqrt(2 * Math.PI * Math.Pow(stDev, 2));

            for (double i = 1; i <= 100; i++)
            {
                var powerNumerator = -Math.Pow((i - mean), 2);
                var powerDenominator = 2 * Math.Pow(stDev, 2);
                var power = powerNumerator / powerDenominator;
                var secondValue = Math.Pow(Math.E, power);

                var result = firstValue * secondValue;

                var probabilityDensity = new ProbabilityDensity
                {   XValue = i,
                    PropabilityDensity = result
                };

                resultList.Add(probabilityDensity);
            }

            return resultList;

        }

        private static double StandardDeviation(List<UserResults> values)
        {
            var resultsList = values.Select(x => Convert.ToDouble(x.Mark)).ToList();
            double avg = resultsList.Average();
            return Math.Sqrt(resultsList.Average(v => Math.Pow(v - avg, 2)));
        }

        private double MeanMark(List<UserResults> values)
        {
            var resultsList = values.Select(x => Convert.ToDouble(x.Mark)).ToList();
            double avg = resultsList.Average();
            return avg;
        }

        private void SetNeighbourLabelName(List<KnnResult> resultList)
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
            var mark = Convert.ToDouble(userResult.Mark);
            var courseworkId = userResult.CourseworkId;
            double percentage = _context.Coursework.First(x => x.Id == courseworkId).Percentage;
            var weighting = percentage / 100;
            var weightedMark = mark * weighting;

            return Convert.ToDecimal(weightedMark);
        }

        public double WeightedMark(List<UserResults> userResultList)
        {
            double weightedTotal = 0;
            var courseworkContext = _context.Coursework.ToList();

            foreach (var result in userResultList)
            {
                var mark = Convert.ToDouble(result.Mark);
                double weighting = courseworkContext.First(x => x.Id == result.CourseworkId).Percentage;
                var weightedMark = mark * (weighting / 100);

                weightedTotal += weightedMark;
            }

            return Math.Round(weightedTotal,2);
        }

        public double WeightedMarkForModule(List<UserModuleResult> userModuleResultList)
        {
            double weightedTotal = 0;
            var moduleContext = _context.Module.ToList();

            foreach (var result in userModuleResultList)
            {
                var mark = Convert.ToDouble(result.Mark);
                double credits = moduleContext.First(x => x.Id == result.ModuleId).Credits;
                var weightedMark = mark * (credits / 120);

                weightedTotal += weightedMark;
            }

            return Math.Round(weightedTotal, 2);
        }

        public List<MarkToClassification> CalculateNeededMarks(List<UserResults> userResultList)
        {
            double totalWeightedMark = 0;
            double totalWeighting = 0;
            foreach (var result in userResultList)
            {
                var weightedMark = Convert.ToDouble(WeightedMark(result));
                totalWeightedMark += weightedMark;

                var weighting = result.Coursework.Percentage;
                totalWeighting += weighting;
            }

            var remainingWeighting = 100 - totalWeighting;
            var markToClassificationList = new List<MarkToClassification>();
            var classificationList = _context.Classification.ToList();

            foreach (var classification in classificationList)
            {
                var distanceToClassification = classification.LowerBound - totalWeightedMark;
                var markToGoal = Math.Round((distanceToClassification / remainingWeighting * 100), 2);

                if (markToGoal < 0)
                {
                    markToGoal = 0;
                }

                var marksToClassification = new MarkToClassification
                {
                    Label = SetClassificationLabelName(Convert.ToInt16(classification.LowerBound)),
                    MarkNeeded = markToGoal
                };

                markToClassificationList.Add(marksToClassification);
            }

            return markToClassificationList;
        }

        private String SetClassificationLabelName(int lowerBound)
        { 
                String label;
                switch (lowerBound)
                {
                    case 0: label = "Fail"; break;
                    case 40: label = "Third Class"; break;
                    case 50: label = "Second Class Division 2"; break;
                    case 60: label = "Second Class Division 1"; break;
                    case 70: label = "First Class"; break;
                    default: label = "Unknown"; break;
                }

            return label;
        }
    }
}