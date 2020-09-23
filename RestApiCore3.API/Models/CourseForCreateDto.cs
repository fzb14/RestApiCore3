using RestApiCore3.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiCore3.API.Models
{
    [CourseTitleMustBeDifferentFromDescription]
    public class CourseForCreateDto //: IValidatableObject
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(1500)]
        public string Description { get; set; }

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult("Title should be different from Description.", new[] { "CourseForCreateDto" });
        //    }
        //}
    }
}
