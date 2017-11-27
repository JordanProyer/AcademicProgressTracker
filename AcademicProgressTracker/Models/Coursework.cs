using System.ComponentModel.DataAnnotations.Schema;

namespace AcademicProgressTracker.Models
{
    public class Coursework
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ModuleId { get; set; }
        public int Percentage { get; set; }

        [ForeignKey("ModuleId")]
        public Module Module { get; set; }

    }
}