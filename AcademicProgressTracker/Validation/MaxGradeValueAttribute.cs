using System.ComponentModel.DataAnnotations;

namespace AcademicProgressTracker.Models
{
    public class MaxGradeValueAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var userResult = (UserResults)validationContext.ObjectInstance;

            if (userResult.Mark <= 100 || userResult.Mark == null)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(" Value can not be greater than 100");
        }
    }
}