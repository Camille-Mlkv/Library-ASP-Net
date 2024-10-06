using Microsoft.AspNetCore.Mvc;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Services.GenreService;

namespace WEB_253502_Melikava.UI.Services.BookService
{
    public class MemoryBookService : IBookService
    {
        private List<Book> _books;
        private List<Genre> _genres;
        private readonly IConfiguration _config;
        private readonly int _itemsPerPage;

        public MemoryBookService([FromServices] IConfiguration config, IGenreService genreService)
        {
            _genres = genreService.GetGenreListAsync().Result.Data;
            _config = config;
            _itemsPerPage = _config.GetValue<int>("ItemsPerPage");
            SetupData();
        }


        /// <summary>
        /// Инициализация списков
        /// </summary>
        private void SetupData()
        {
            _books = new List<Book>
            {
                 new Book 
                 {
                     Id = 1, 
                     Title="Граф Монте-Кристо",
                     Description="Невероятная история о том, на что способна месть, движимая разумом.Attendre et esperer!",
                     Price =60, 
                     Image="Images/Comte.jpg", 
                     GenreId=(_genres.Find(g=>g.NormalizedName.Equals("classic"))).Id,
                 },
                 new Book
                 {
                     Id = 2,
                     Title="Отверженные",
                     Description="Плохие времена создают сильных людей - Жан Вальжан.",
                     Price =100,
                     Image="Images/Miserables.jpg",
                     GenreId=(_genres.Find(g=>g.NormalizedName.Equals("classic"))).Id,
                 },
                 new Book
                 {
                     Id = 3,
                     Title="Убийство в Восточном экспрессе",
                     Description="Одно из самых гениальных разоблачений Эркюля Пуаро.",
                     Price =40,
                     Image="Images/Express.jpg",
                     GenreId=(_genres.Find(g=>g.NormalizedName.Equals("detectives"))).Id,
                 },
                 new Book
                 {
                     Id = 4,
                     Title="Кладбище домашних животных",
                     Description="Сказать, что жутко - ничего не сказать. Книга оставит впечатление навсегда.",
                     Price =50,
                     Image="Images/Pets.jpg",
                     GenreId=(_genres.Find(g=>g.NormalizedName.Equals("horrors"))).Id,
                 },
                 new Book
                 {
                     Id = 5,
                     Title="Мастер и Маргарита",
                     Description="Аннушка уже купила подсолнечное масло, и не только купила, но даже разлила...",
                     Price =60,
                     Image="Images/MM.jpg",
                     GenreId=(_genres.Find(g=>g.NormalizedName.Equals("fantasy"))).Id,
                 },
                 new Book
                 {
                     Id = 6,
                     Title="Три товарища",
                     Description="История о дружбе, любви и потерях в тени послевоенной реальности.",
                     Price =30,
                     Image="Images/ThreeFriends.jpg",
                     GenreId=(_genres.Find(g=>g.NormalizedName.Equals("classic"))).Id,
                 },
                 new Book
                 {
                     Id = 7,
                     Title="Моби Дик или Белый Кит",
                     Description="Эпическая охота за неуловимым китом, отражающая борьбу человека с природой.",
                     Price =50,
                     Image="Images/Moby_Dick.jpg",
                     GenreId=(_genres.Find(g=>g.NormalizedName.Equals("classic"))).Id,
                 },
            };
        }




        public Task<ResponseData<Book>> CreateProductAsync(Book product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }

        public Task DeleteProductAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<BookListModel<Book>>> GetProductListAsync(string? genreNormalizedName, int pageNo = 1)
        {
            var genre = _genres.FirstOrDefault(g => g.NormalizedName == genreNormalizedName);
            List<Book> filteredBooks=new List<Book>();
            if (genre!=null)
            {
                int genreId = (_genres.FirstOrDefault(g => g.NormalizedName == genreNormalizedName)).Id;
                filteredBooks=_books.Where(b=>b.GenreId==genreId).ToList();
            }
            else
            {
                filteredBooks = _books;
            }

            int totalBooks = filteredBooks.Count;

            var totalPages = (int)Math.Ceiling((double)totalBooks / _itemsPerPage);

            var pagedProducts = filteredBooks
                .Skip((pageNo - 1) * _itemsPerPage)
                .Take(_itemsPerPage)
                .ToList();

            // Формируем ответ
            var result = new ResponseData<BookListModel<Book>>
            {
                Data = new BookListModel<Book>
                {
                    Items = pagedProducts,
                    CurrentPage = pageNo,
                    TotalPages = totalPages
                },
                Successfull = true,
            };

            return await Task.FromResult(result);
        }

        public Task UpdateProductAsync(int id, Book product, IFormFile? formFile)
        {
            throw new NotImplementedException();
        }
    }
}
