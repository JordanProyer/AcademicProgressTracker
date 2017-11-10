using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AcademicProgressTracker.Models
{
    public class UserModules
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ModuleId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [ForeignKey("ModuleId")]
        public Module Module { get; set; }
    }
}