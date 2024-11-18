using NSubstitute;
using Microsoft.AspNetCore.Mvc;
using WEB_253502_Melikava.UI.Controllers;
using WEB_253502_Melikava.UI.Services.BookService;
using WEB_253502_Melikava.UI.Services.GenreService;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Microsoft.Extensions.Primitives;


namespace WEB_253502_Melikava.Tests
{
    public class BookControllerIndexMethodTests
    {
        private readonly BookController _controller;
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;

        public BookControllerIndexMethodTests()
        {
            _bookService=Substitute.For<IBookService>();
            _genreService=Substitute.For<IGenreService>();
            _controller = new BookController(_genreService, _bookService);
        }

        [Fact]
        public async void GetGenreList_RetrieveFailed_ReturnsNotFound()
        {
            // Arrange
            _genreService.GetGenreListAsync().Returns(new ResponseData<List<Genre>> { Successfull = false });

            // Act
            var result = await _controller.Index(null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async void GetBookList_RetrieveFailed_ReturnsNotFound()
        {
            // Arrange
            _genreService.GetGenreListAsync().Returns(new ResponseData<List<Genre>> { Successfull = true });
            _bookService.GetProductListAsync(It.IsAny<string?>()).Returns(new ResponseData<BookListModel<Book>> { Successfull = false });

            // Act
            var result = await _controller.Index(null);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetBookList_SortedList_ReturnsView()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { NormalizedName = "Fiction", Name = "Fiction" },
                new Genre { NormalizedName = "NonFiction", Name = "Non-Fiction" }
            };
            _genreService.GetGenreListAsync().Returns(new ResponseData<List<Genre>> { Successfull = true, Data = genres });

            var books = new BookListModel<Book>
            {
                Items = new List<Book> { new Book { Title = "Book1" }, new Book { Title = "Book2" } },
                TotalPages = 1
            };
            _bookService.GetProductListAsync("Fiction", 1)
                .Returns(new ResponseData<BookListModel<Book>> { Successfull = true, Data = books });


            //var header = new Dictionary<string, StringValues>
            //{
            //    ["x-requested-with"] = "XMLHttpRequest"
            //};
            var moqHttpContext = new Mock<HttpContext>(); // without header
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = moqHttpContext.Object
            };

            // Act
            var result = await _controller.Index("Fiction") as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Same(books, result.Model);

            var viewData = result.ViewData;
            Assert.Equal(genres, viewData["Genres"]);
            Assert.Equal(genres.First(), viewData["CurrentGenre"]);
        }

        [Fact]
        public async Task GetBookList_WholeList_ReturnsView()
        {
            // Arrange
            var genres = new List<Genre>
            {
                new Genre { NormalizedName = "Fiction", Name = "Fiction" },
                new Genre { NormalizedName = "NonFiction", Name = "Non-Fiction" }
            };
            _genreService.GetGenreListAsync().Returns(new ResponseData<List<Genre>> { Successfull = true, Data = genres });

            var books = new BookListModel<Book>
            {
                Items = new List<Book> { new Book { Title = "Book1" }, new Book { Title = "Book2" } },
                TotalPages = 1
            };
            _bookService.GetProductListAsync(null, 1)
                .Returns(new ResponseData<BookListModel<Book>> { Successfull = true, Data = books });

            var moqHttpContext = new Mock<HttpContext>(); // without header
            moqHttpContext.Setup(c => c.Request.Headers).Returns(new HeaderDictionary());

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = moqHttpContext.Object
            };

            // Act
            var result = await _controller.Index(null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Same(books, result.Model);

            var viewData = result.ViewData;
            Assert.Equal(genres, viewData["Genres"]);
            Assert.Null(viewData["CurrentGenre"]);
        }
    }
}