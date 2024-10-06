using Microsoft.EntityFrameworkCore;
using WEB_253502_Melikava.API.Data;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;

namespace WEB_253502_Melikava.API.Services.GenreService
{
    public class GenreService : IGenreService
    {
        private AppDbContext _db;
        public GenreService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ResponseData<Genre>> CreateGenreAsync(Genre genre)
        {
            if (genre == null)
            {
                return ResponseData<Genre>.Error("Genre cannot be null");
            }

            var existingGenre = _db.Genres.FirstOrDefault(g => g.NormalizedName == genre.NormalizedName);
            if (existingGenre != null)
            {
                return ResponseData<Genre>.Error("Genre with the same name already exists");
            }

            // Добавляем новый жанр
            await _db.Genres.AddAsync(genre);
            await _db.SaveChangesAsync();

            return ResponseData<Genre>.Success(genre);
        }

        public async Task<ResponseData<List<Genre>>> GetGenreListAsync()
        {
            var genres = await _db.Genres.ToListAsync();

            if (genres == null || !genres.Any())
            {
                return ResponseData<List<Genre>>.Error("No genres found");
            }

            return ResponseData<List<Genre>>.Success(genres);
        }
    }
}
