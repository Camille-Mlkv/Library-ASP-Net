using Microsoft.EntityFrameworkCore;
using WEB_253502_Melikava.Domain.Entities;

namespace WEB_253502_Melikava.API.Data
{
    public class DbInitializer
    {
        public static async Task SeedData(WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var configuration = app.Configuration;
            var apiBaseUrl = configuration["AppSettings:ApiBaseUrl"];

            // Add genres
            await context.Genres.AddAsync(new Genre { Name = "Фантастика", NormalizedName = "fantastics" });
            await context.Genres.AddAsync(new Genre { Name = "Фэнтэзи", NormalizedName = "fantasy" });
            await context.Genres.AddAsync(new Genre { Name = "Детективы", NormalizedName = "detectives" });
            await context.Genres.AddAsync(new Genre { Name = "Триллеры", NormalizedName = "trillers" });
            await context.Genres.AddAsync(new Genre { Name = "Приключения", NormalizedName = "adventures" });
            await context.Genres.AddAsync(new Genre { Name = "Ужасы", NormalizedName = "horrors" });
            await context.Genres.AddAsync(new Genre { Name = "Классическая литература", NormalizedName = "classic" });

            await context.SaveChangesAsync();

            //Add books
            await context.Books.AddAsync(
                new Book
                {
                    Title = "Граф Монте-Кристо",
                    Description = "Невероятная история о том, на что способна месть, движимая разумом.Attendre et esperer!",
                    Price = 60,
                    Image = $"{apiBaseUrl}/Images/Comte.jpg",
                    GenreId = (await context.Genres.FirstOrDefaultAsync(g => g.NormalizedName.Equals("classic"))).Id
                }
            );

            await context.Books.AddAsync(
                new Book
                {
                    Title = "Отверженные",
                    Description = "Плохие времена создают сильных людей - Жан Вальжан.",
                    Price = 100,
                    Image = $"{apiBaseUrl}/Images/Miserables.jpg",
                    GenreId = (await context.Genres.FirstOrDefaultAsync(g => g.NormalizedName.Equals("classic"))).Id
                }
            );

            await context.Books.AddAsync(
                new Book
                {
                    Title = "Убийство в Восточном экспрессе",
                    Description = "Одно из самых гениальных разоблачений Эркюля Пуаро.",
                    Price = 40,
                    Image = $"{apiBaseUrl}/Images/Express.jpg",
                    GenreId = (await context.Genres.FirstOrDefaultAsync(g => g.NormalizedName.Equals("detectives"))).Id
                }
            );

            await context.Books.AddAsync(
                new Book
                {
                    Title = "Кладбище домашних животных",
                    Description = "Сказать, что жутко - ничего не сказать. Книга оставит впечатление навсегда.",
                    Price = 50,
                    Image = $"{apiBaseUrl}/Images/Pets.jpg",
                    GenreId = (await context.Genres.FirstOrDefaultAsync(g => g.NormalizedName.Equals("horrors"))).Id
                }
            );

            await context.Books.AddAsync(
                new Book
                {
                    Title = "Мастер и Маргарита",
                    Description = "Аннушка уже купила подсолнечное масло, и не только купила, но даже разлила...",
                    Price = 60,
                    Image = $"{apiBaseUrl}/Images/MM.jpg",
                    GenreId = (await context.Genres.FirstOrDefaultAsync(g => g.NormalizedName.Equals("fantasy"))).Id
                }
            );

            await context.Books.AddAsync(
                new Book
                {
                    Title = "Три товарища",
                    Description = "История о дружбе, любви и потерях в тени послевоенной реальности.",
                    Price = 30,
                    Image = $"{apiBaseUrl}/Images/ThreeFriends.jpg",
                    GenreId = (await context.Genres.FirstOrDefaultAsync(g => g.NormalizedName.Equals("classic"))).Id
                }
            );

            await context.Books.AddAsync(
                new Book
                {
                    Title = "Моби Дик или Белый Кит",
                    Description = "Эпическая охота за неуловимым китом, отражающая борьбу человека с природой.",
                    Price = 50,
                    Image = $"{apiBaseUrl}/Images/Moby_Dick.jpg",
                    GenreId = (await context.Genres.FirstOrDefaultAsync(g => g.NormalizedName.Equals("classic"))).Id
                }
            );

            await context.SaveChangesAsync();
        }
    }
}
