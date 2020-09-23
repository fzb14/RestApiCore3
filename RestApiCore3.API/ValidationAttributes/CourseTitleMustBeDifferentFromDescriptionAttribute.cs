using Microsoft.Extensions.Options;
using RestApiCore3.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiCore3.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var course = (CourseForCreateDto)validationContext.ObjectInstance;
            if (course.Title == course.Description)
            {
                return new ValidationResult("Title should be different from Description.", new[] { nameof(CourseForCreateDto) });
            }
            return ValidationResult.Success;
        }
    }
}
