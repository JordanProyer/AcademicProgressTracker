﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class SetupViewModel
    {
        //For dropdowns
        public IEnumerable<Course> Course { get; set; }
        public IEnumerable<Year> Year { get; set; }
        public IEnumerable<Module> Module { get; set; }

        //Post back to controller
        public IEnumerable<int> ModuleId { get; set; }
        public int CourseId { get; set; }
        public int YearId { get; set; }
        public bool Success { get; set; }
        public bool SelectionComplete { get; set; }

    }
}