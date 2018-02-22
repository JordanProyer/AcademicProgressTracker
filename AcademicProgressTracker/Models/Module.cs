using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicProgressTracker.Models
{
    public class Module
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int CourseId { get; set; }
        public int YearId { get; set; }
        public int Credits { get; set; }
        public bool Optional { get; set; }

        [ForeignKey("CourseId")]
        public Course Course { get; set; }
        [ForeignKey("YearId")]
        public Year Year { get; set; }
    }
}