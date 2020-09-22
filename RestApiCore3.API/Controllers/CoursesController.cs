using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiCore3.API.Models;
using RestApiCore3.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiCore3.API.Controllers
{
    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CoursesController : ControllerBase
    {
        private readonly IRestApiCore3Repository repository;
        private readonly IMapper mapper;

        public CoursesController(IRestApiCore3Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courses = repository.GetCourses(authorId);
            return Ok(mapper.Map<IEnumerable<CourseDto>>(courses));
        }
    }

    
}
