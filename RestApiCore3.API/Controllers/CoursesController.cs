using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using RestApiCore3.API.Entities;
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
        [HttpGet("{courseId}", Name ="GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var course = repository.GetCourse(authorId, courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<CourseDto>(course));
        }
        [HttpPost]
        public ActionResult<CourseDto> CreateCourseForAuthor(Guid authorId, CourseForCreateDto courseForCreate)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = mapper.Map<Course>(courseForCreate);
            repository.AddCourse(authorId, courseEntity);
            repository.Save();
            var courseToReturn = mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourseForAuthor", new { authorId = authorId, courseId = courseToReturn.Id }, courseToReturn);
        }
        [HttpPut("{courseId}")]
        public IActionResult UpdateCourseForAuthor(Guid authorId, Guid courseId, CourseForUpdateDto courseForUpdate)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = repository.GetCourse(authorId, courseId);
            if (courseEntity == null)
            {
                courseEntity = mapper.Map<Course>(courseForUpdate);
                courseEntity.Id = courseId;
                repository.AddCourse(authorId,courseEntity);
                repository.Save();
                var courseToReturn = mapper.Map<CourseDto>(courseEntity);
                return CreatedAtRoute("GetCourseForAuthor", new { authorId, courseId = courseToReturn.Id }, courseToReturn);

            }
            mapper.Map(courseForUpdate, courseEntity);
            repository.UpdateCourse(courseEntity);
            repository.Save();
            var courseDto = mapper.Map<CourseDto>(courseEntity);
            return Ok(courseDto);
        }
        [HttpPatch("{courseId}")]
        public  ActionResult PartiallyUpdateCourseForAuthor(Guid authorId, Guid courseId, JsonPatchDocument<CourseForUpdateDto> patchDocument)
        {
            if (!repository.AuthorExists(authorId))
            {
                return NotFound();
            }
            var courseEntity = repository.GetCourse(authorId, courseId);
            if (courseEntity == null)
            {
                return NotFound();
            }
            var courseToPatch = mapper.Map<CourseForUpdateDto>(courseEntity);
            patchDocument.ApplyTo(courseToPatch, ModelState);
            //have to validate manually when update with patch
            if (!TryValidateModel(courseToPatch)) {
                return ValidationProblem(ModelState);
            }

            mapper.Map(courseToPatch, courseEntity);
            repository.UpdateCourse(courseEntity);
            repository.Save();
            return NoContent();
        }

    }

    
}
