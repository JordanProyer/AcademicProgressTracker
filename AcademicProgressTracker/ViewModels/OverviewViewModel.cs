using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class OverviewViewModel
    {
        public string YearName { get; set; }
        public List<UserModuleResult> ModuleResultList { get; set; }
        public List<OverallResult> OverallResultList { get; set; }
    }
}