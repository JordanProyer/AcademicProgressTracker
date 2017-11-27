using System.Collections.Generic;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class GradesAddViewModel
    {
        public Module Module { get; set; }
        public IList<Coursework> CourseworkList { get; set; }
        public IList<UserResults> UserResults { get; set; }

        public bool Success { get; set; }
    }
}
