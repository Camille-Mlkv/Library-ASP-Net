using WEB_253502_Melikava.Domain.Entities;

namespace WEB_253502_Melikava.BlazorWasm.Services
{
    public interface IDataService
    {
        // Событие, генерируемое при изменении данных 
        event Action DataLoaded;
        // Список категорий объектов 
        List<Genre> Genres { get; set; }
        //Список объектов 
        List<Book> Books { get; set; }
        // Признак успешного ответа на запрос к Api 
        bool Success { get; set; }
        // Сообщение об ошибке 
        string? ErrorMessage { get; set; }
        // Количество страниц списка 
        int TotalPages { get; set; }
        // Номер текущей страницы 
        int CurrentPage { get; set; }
        // Фильтр по категории 
        Genre? SelectedGenre { get; set; }

        /// <summary>
        ///  Получение списка всех объектов 
        /// </summary> 
        /// <param name="pageNo">номер страницы списка</param> 
        /// <returns></returns> 
        public Task GetBookListAsync(string? genreNormalizedName, int pageNo = 1);

        /// <summary> 
        /// Получение списка категорий 
        /// </summary> 
        /// <returns></returns> 
        public Task GetGenreListAsync();
    }
}
