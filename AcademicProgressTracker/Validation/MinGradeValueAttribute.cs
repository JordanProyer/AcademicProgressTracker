using System.ComponentModel.DataAnnotations;

namespace AcademicProgressTracker.Models
{
    public class MinGradeValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userResult = (UserResults) validationContext.ObjectInstance;

            if (userResult.Mark >= 0 || userResult.Mark == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(" Value can not be less than zero");
        }
    }
}