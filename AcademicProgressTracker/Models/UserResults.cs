using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicProgressTracker.Models
{
    public class UserResults
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CourseworkId { get; set; }
        public int? Mark { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("CourseworkId")]
        public Coursework Coursework { get; set; }
    }
}