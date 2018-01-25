using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using AcademicProgressTracker.Models;
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
        public double KNeareastNeighbour(int userId, int moduleId)
        {
            var userResult = _context.UserResults.Where(x => x.UserId == userId && x.Coursework.ModuleId == moduleId).ToList();
            var allUserResults = _context.UserResults.Where(x => x.Coursework.ModuleId == moduleId && x.UserId != userId).ToList();

            foreach (var result in allUserResults.ToList())
            {
                if (result.Mark == null)
                {
                    allUserResults.RemoveAll(x => x.UserId == result.UserId);
                }
            }

            List<double> values = new List<double>();
            double shortestDistance = 10000000;
            var userIndex = 0;

            foreach (var group in allUserResults.GroupBy(x => x.UserId))
            {
                foreach (var coursework in allUserResults.Where(x => x.UserId == group.Key))
                {
                    var userMark = Convert.ToDouble(userResult.First(x => x.Mark != null && x.CourseworkId == coursework.CourseworkId).Mark);
                    var distance = Math.Pow(userMark - Convert.ToDouble(coursework.Mark), 2);
                    values.Add(distance);
                }

                var totalDistance = KNNFactor(values);
                if (totalDistance < shortestDistance)
                {
                    userIndex = group.Key;
                    shortestDistance = totalDistance;
                }

                values.Clear();

            }

            var courseworkCount = allUserResults.Count(x => x.UserId == userIndex);

            if (courseworkCount > 0)
            {
                var totalMark = allUserResults.Where(x => x.UserId == userIndex).Select(y => y.Mark).Sum();
                var averageMark = totalMark / courseworkCount;
                return Convert.ToDouble(averageMark);
            }

            return 0;
        }

        private double KNNFactor(List<double> numberList)
        {
            var total = numberList.Sum();
            var distance = Math.Sqrt(total);
            return distance;
        }
    }
}