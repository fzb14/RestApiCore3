﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestApiCore3.API.DbContexts;
using RestApiCore3.API.Entities;
using RestApiCore3.API.Helpers;
using RestApiCore3.API.Models;
using RestApiCore3.API.Services;
using RestApiCore3.API.SourceParameters;

namespace RestApiCore3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IRestApiCore3Repository _repos;
        private readonly IMapper mapper;

        public AuthorsController(IRestApiCore3Repository repos, IMapper mapper)
        {
            _repos = repos;
            this.mapper = mapper;
        }

        // GET: api/Authors
        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery]AuthorSourchParameters parameters)
        {
            //throw new Exception("test exception.");
            //var authors = new List<AuthorDto>();
            //foreach(var a in _repos.GetAuthors())
            //{
            //    authors.Add(new AuthorDto
            //    {
            //        Id=a.Id,
            //        Name = $"{a.FirstName} {a.LastName}",
            //        MainCategory = a.MainCategory,
            //        Age = a.DateOfBirth.GetAge()
            //    });
            //}
            var source = _repos.GetAuthors(parameters?.MainCategory, parameters?.SearchQuery);

            var authors = mapper.Map<IEnumerable<AuthorDto>>(source);
            return Ok(authors);
        }

        // GET: api/Authors/5
        [HttpGet("{id}", Name ="GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid id)
        {
            var author = _repos.GetAuthor(id);

            if (author == null)
            {
                return NotFound();
            }
            //var authorDto = new AuthorDto
            //{
            //    Id = author.Id,
            //    Name = $"{author.FirstName} {author.LastName}",
            //    Age = author.DateOfBirth.GetAge(),
            //    MainCategory = author.MainCategory
            //};
            var authorDto = mapper.Map<AuthorDto>(author);
            return Ok(authorDto);
        }

        // PUT: api/Authors/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutAuthor(Guid id, Author author)
        //{
        //    if (id != author.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(author).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!AuthorExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Authors
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public ActionResult<AuthorDto> PostAuthor(AuthorForCreateDto author)
        {
            var authorEntity = mapper.Map<Author>(author);
            _repos.AddAuthor(authorEntity);
            _repos.Save();
            var authorResult = mapper.Map<AuthorDto>(authorEntity);
            return CreatedAtAction("GetAuthor", new { id = authorResult.Id }, authorResult);
        }

        //// DELETE: api/Authors/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Author>> DeleteAuthor(Guid id)
        //{
        //    var author = await _context.Authors.FindAsync(id);
        //    if (author == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Authors.Remove(author);
        //    await _context.SaveChangesAsync();

        //    return author;
        //}

        //private bool AuthorExists(Guid id)
        //{
        //    return _context.Authors.Any(e => e.Id == id);
        //}

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }
    }
}
