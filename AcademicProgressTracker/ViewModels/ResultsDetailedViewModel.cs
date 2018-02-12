using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class ResultsDetailedViewModel
    {
        public Module Module { get; set; }
        public double KnnPredictionNum { get; set; }
        public String KnnPredictionTxt { get; set; }
        public int kValue { get; set; }
    }
}