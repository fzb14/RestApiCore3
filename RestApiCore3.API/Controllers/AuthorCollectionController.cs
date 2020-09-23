using AutoMapper;
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
    [Route("api/authorcollections")]
    public class AuthorCollectionController : ControllerBase
    {
        private readonly IRestApiCore3Repository repository;
        private readonly IMapper mapper;

        public AuthorCollectionController(IRestApiCore3Repository repository, IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }

        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateAuthorCollection(IEnumerable<AuthorForCreateDto> authorCollection)
        {
            var authorEntityCollection = mapper.Map<IEnumerable<Author>>(authorCollection);
            foreach(var author in authorEntityCollection)
            {
                repository.AddAuthor(author);
            }
            repository.Save();

            return Ok();
        }
    }
}
