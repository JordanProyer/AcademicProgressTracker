using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using AcademicProgressTracker.Models;

namespace AcademicProgressTracker.Validation
{
    public class MinCreditsAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var module = (Module)validationContext.ObjectInstance;

            if (module.Credits > 0)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(" Value can not be less than 0");
        }
    }
}