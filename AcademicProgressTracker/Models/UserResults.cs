using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicProgressTracker.Models
{
    public class UserResults
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseworkId { get; set; }

        [MinGradeValue]
        [MaxGradeValue]
        [DisplayName("Result")]
        public decimal? Mark { get; set; }
        [DisplayName("Submission Date")]
        public DateTime AddedDateTime { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("CourseworkId")]
        public Coursework Coursework { get; set; }

    }
}