using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class GradesViewModel
    {
        public IEnumerable<Module> ModuleList { get; set; }
    }
}