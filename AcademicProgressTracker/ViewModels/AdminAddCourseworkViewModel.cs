using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.ViewModels
{
    public class AdminAddCourseworkViewModel
    {
        public Module Module { get; set; }
        public List<Coursework> Coursework { get; set; }
        public int Index { get; set; }
    }
}