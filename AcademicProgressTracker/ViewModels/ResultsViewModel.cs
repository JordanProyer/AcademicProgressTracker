using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class ResultsViewModel
    {
        public string ModuleName { get; set; }
        public int ModuleId { get; set; }
        public int CompletedCoursework { get; set; }
        public int TotalCourseworks { get; set; }
        public decimal? AverageMark { get; set; }
        public string PredictedClassification { get; set; }
    }
}