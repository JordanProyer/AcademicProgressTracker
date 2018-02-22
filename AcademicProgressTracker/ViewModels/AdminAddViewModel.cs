using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class AdminAddViewModel
    {
        //Populate dropdowns
        [DisplayName("Course Type")]
        public List<string> CourseType { get; set; }
        [DisplayName("Course Duration")]
        public List<int> CourseDuration { get; set; }
        public List<Year> YearList { get; set; }

        //Post back to controller
        [DisplayName("Course Name")]
        public string CourseName { get; set; }
        public List<Module> Modules { get; set; }
        public int YearId { get; set; }


    }
}