using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RestApiCore3.API.Entities;
using RestApiCore3.API.Helpers;
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
        [HttpGet("({ids})", Name ="getauthorcollection")]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthorCollection(
            [FromRoute][ModelBinder(BinderType =typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
                return BadRequest();
            var authorCollection = repository.GetAuthors(ids);
            if (ids.Count() != authorCollection.Count())
                return NotFound();
            return Ok(mapper.Map<IEnumerable<AuthorDto>>(authorCollection));
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
            var authorDtoCollection = mapper.Map<IEnumerable<AuthorDto>>(authorEntityCollection);
            var ids = string.Join(",", authorEntityCollection.Select(a => a.Id));
            return CreatedAtRoute("getauthorcollection",new { ids = ids}, authorDtoCollection);
        }
    }
}
