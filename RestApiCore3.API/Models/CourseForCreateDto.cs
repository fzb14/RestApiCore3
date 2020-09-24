using RestApiCore3.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiCore3.API.Models
{
    
    public class CourseForCreateDto : CourseForManipulationDto //: IValidatableObject
    {
        

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult("Title should be different from Description.", new[] { "CourseForCreateDto" });
        //    }
        //}
    }
}
