using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiCore3.API.Models
{
    public class CourseForUpdateDto : CourseForManipulationDto
    {
        //public string Title { get; set; }
        //public string Description { get; set; }
        [Required(ErrorMessage ="Description is required when update course.")]
        public override string Description { get => base.Description; set => base.Description = value; }
    }
}
