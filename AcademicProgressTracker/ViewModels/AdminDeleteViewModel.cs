using System.Collections.Generic;
using System.ComponentModel;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class AdminDeleteViewModel
    {
        public List<Course> CourseList { get; set; }

        [DisplayName("Course to Delete")]
        public int CourseToDeleteId { get; set; }
        public bool Success { get; set; }
    }
}