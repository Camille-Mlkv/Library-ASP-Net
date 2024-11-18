using Microsoft.EntityFrameworkCore;
using WEB_253502_Melikava.API.Data;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.API.Services.BookService;
using WEB_253502_Melikava.Domain.Entities;

namespace WEB_253502_Melikava.Tests
{
    public class BookServiceGetBooksListTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new AppDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public void GetBookList_DefaultFirstPage_Successful()
        {
            //Arrange
            using var context = CreateContext();
            SeedData(context);
            var service = new BookService(context);

            //Act
            var result = service.GetProductListAsync(null).Result;

            //Assert
            Assert.IsType<ResponseData<BookListModel<Book>>>(result);
            Assert.True(result.Successfull);
            Assert.Equal(1, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
            Assert.Equal(2, result.Data.TotalPages);
            Assert.Equal(context.Books.First(), result.Data.Items[0]);
        }

        [Fact]
        public async Task GetBookList_AppropriatePage_Successful()
        {
            //Arrange
            using var context = CreateContext();
            SeedData(context);
            var service = new BookService(context);

            //Act
            var result = await service.GetProductListAsync(null, pageNo: 2);

            //Assert
            Assert.True(result.Successfull);
            Assert.Equal(2, result.Data.CurrentPage);
            Assert.Equal(3, result.Data.Items.Count);
        }

        [Fact]
        public async Task GetBookList_FilteredByGenre_Successful()
        {
            //Arrange
            using var context = CreateContext();
            SeedData(context);
            var service = new BookService(context);

            //Act
            var result = await service.GetProductListAsync("FICTION");

            //Assert
            Assert.True(result.Successfull);
            Assert.All(result.Data.Items, item => Assert.Equal(context.Genres.First(g => g.NormalizedName == "FICTION").Id, item.GenreId));
        }

        [Fact]
        public async Task GetBookList_LimitsPageSize_Successful()
        {
            //Arrange
            using var context = CreateContext();
            SeedData(context);
            var service = new BookService(context);
            var pageSizeInput = 30;

            //Act
            var result = await service.GetProductListAsync(null, pageSize: pageSizeInput);

            //Assert
            Assert.True(result.Successfull);
            Assert.True(result.Data.Items.Count <= 20);
        }

        [Fact]
        public async Task GetBookList_InvalidPage_ReturnsError()
        {
            //Arrange
            using var context = CreateContext();
            SeedData(context);
            var service = new BookService(context);

            //Act
            var result = await service.GetProductListAsync(null, pageNo: 10);

            //Assert
            Assert.False(result.Successfull);
            Assert.Equal("No such page", result.ErrorMessage);
        }


        private void SeedData(AppDbContext context)
        {
            var genre1 = new Genre { Id = 1, Name = "Fiction", NormalizedName = "FICTION" };
            var genre2 = new Genre { Id = 2, Name = "Non-Fiction", NormalizedName = "NON-FICTION" };

            context.Genres.AddRange(genre1, genre2);

            for (int i = 1; i <= 6; i++)
            {
                context.Books.Add(new Book
                {
                    Id = i,
                    Title = $"Book {i}",
                    Author=$"Auhtor {i}",
                    Description="Best",
                    Price=i,
                    GenreId = i <= 3 ? genre1.Id : genre2.Id
                });
            }

            context.SaveChanges();
        }
    }

    

}
