﻿using Microsoft.Extensions.Options;
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
            var course = (CourseForManipulationDto)validationContext.ObjectInstance;
            if (course.Title == course.Description)
            {
                return new ValidationResult(ErrorMessage, new[] { nameof(CourseForManipulationDto) });
            }
            return ValidationResult.Success;
        }
    }
}
