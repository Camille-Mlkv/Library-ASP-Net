using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;

namespace WEB_253502_Melikava.API.Services.GenreService
{
    public interface IGenreService
    {
        /// <summary>
        /// Получение списка всех категорий
        /// </summary>
        /// <returns></returns>
        public Task<ResponseData<List<Genre>>> GetGenreListAsync();
        public Task<ResponseData<Genre>> CreateGenreAsync(Genre genre);
    }
}
