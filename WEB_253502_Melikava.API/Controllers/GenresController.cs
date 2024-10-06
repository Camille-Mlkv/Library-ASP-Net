using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Humanizer.Localisation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using WEB_253502_Melikava.API.Data;
using WEB_253502_Melikava.API.Services.GenreService;
using WEB_253502_Melikava.Domain.Entities;

namespace WEB_253502_Melikava.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IGenreService _genreService;

        public GenresController(AppDbContext context,IGenreService genreService)
        {
            _context = context;
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            return Ok(await _genreService.GetGenreListAsync());
        }

        [HttpPost]
        public async Task<ActionResult<Genre>> PostGenre(Genre genre)
        {
            return Ok(await _genreService.CreateGenreAsync(genre));
        }


        //[HttpGet("{id}")]
        //public async Task<ActionResult<Genre>> GetGenre(int id)
        //{
        //    var genre = await _context.Genres.FindAsync(id);

        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }

        //    return genre;
        //}


        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutGenre(int id, Genre genre)
        //{
        //    if (id != genre.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(genre).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!GenreExists(id))
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

        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteGenre(int id)
        //{
        //    var genre = await _context.Genres.FindAsync(id);
        //    if (genre == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Genres.Remove(genre);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool GenreExists(int id)
        //{
        //    return _context.Genres.Any(e => e.Id == id);
        //}
    }
}
