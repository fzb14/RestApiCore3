using AutoMapper;
using RestApiCore3.API.Entities;
using RestApiCore3.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiCore3.API.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CourseForCreateDto, Course>();
            CreateMap<CourseForUpdateDto, Course>().ReverseMap();
        }
    }
}
