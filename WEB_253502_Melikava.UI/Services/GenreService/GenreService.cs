using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;

namespace WEB_253502_Melikava.UI.Services.GenreService
{
    public class GenreService : IGenreService
    {
        public Task<ResponseData<List<Genre>>> GetGenreListAsync()
        {
            var categories = new List<Genre>
            {
                new Genre {Id=1, Name="Фантастика",NormalizedName="fantastics"},
                new Genre {Id=2, Name="Фэнтэзи",NormalizedName="fantasy"},
                new Genre {Id=3, Name="Детективы", NormalizedName="detectives"},
                new Genre {Id=4, Name="Триллеры",NormalizedName="trillers"},
                new Genre {Id=5, Name="Приключения",NormalizedName="adventures"},
                new Genre {Id=6, Name="Ужасы",NormalizedName="horrors"},
                new Genre {Id=7, Name="Классическая литература",NormalizedName="classic"},
            };
            var result = ResponseData<List<Genre>>.Success(categories);
            return Task.FromResult(result);
        }
    }
}
