using Microsoft.EntityFrameworkCore;
using WEB_253502_Melikava.API.Data;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;

namespace WEB_253502_Melikava.API.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly int _maxPageSize = 20;
        private AppDbContext _db;
        public BookService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<ResponseData<BookListModel<Book>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1, int pageSize = 3)
        {
            var genre = _db.Genres.FirstOrDefault(g => g.NormalizedName == categoryNormalizedName);
            if (pageSize > _maxPageSize)
            {
                pageSize = _maxPageSize;
            }

            var query = _db.Books.AsQueryable();
            var dataList = new BookListModel<Book>();
            query = query.Where(g => categoryNormalizedName == null || g.GenreId == genre.Id);

            // количество элементов в списке
            var count = await query.CountAsync();
            if (count == 0)
            {
                return ResponseData<BookListModel<Book>>.Success(dataList);
            }
            // количество страниц
            int totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (pageNo > totalPages)
            {
                return ResponseData<BookListModel<Book>>.Error("No such page");
            }

            dataList.Items = await query.OrderBy(d => d.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();
            dataList.CurrentPage = pageNo;
            dataList.TotalPages = totalPages;
            return ResponseData<BookListModel<Book>>.Success(dataList);

        }

        public async Task<ResponseData<Book>> CreateProductAsync(Book product)
        {
            if (product == null)
            {
                return ResponseData<Book>.Error("Product cannot be null");
            }

            await _db.Books.AddAsync(product);
            await _db.SaveChangesAsync();

            return ResponseData<Book>.Success(product);
        }

        public async Task DeleteProductAsync(int id)
        {
            var product = await _db.Books.FindAsync(id);
            if (product == null)
            {
                throw new Exception($"Book with ID {id} not found");
            }

            _db.Books.Remove(product);
            await _db.SaveChangesAsync();

        }

        public async Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            var product = await _db.Books.FindAsync(id);
            if (product == null)
            {
                return ResponseData<Book>.Error($"Book with ID {id} not found");
            }

            return ResponseData<Book>.Success(product);
        }

        public async Task<ResponseData<string>> SaveImageAsync(int id, IFormFile formFile)
        {
            var product = await _db.Books.FindAsync(id);
            if (product == null)
            {
                return ResponseData<string>.Error($"Book with ID {id} not found");
            }

            if (formFile == null || formFile.Length == 0)
            {
                return ResponseData<string>.Error("Invalid image file");
            }

            var filePath = Path.Combine("wwwroot", "Images", formFile.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            product.Image = $"https://localhost:7002/Images/{formFile.FileName}";
            await _db.SaveChangesAsync();

            return ResponseData<string>.Success(product.Image);
        }

        public async Task UpdateProductAsync(int id, Book product)
        {
            var existingProduct = await _db.Books.FindAsync(id);
            if (existingProduct == null)
            {
                throw new Exception($"Book with ID {id} not found");
            }

            existingProduct.Title = product.Title;
            existingProduct.Description = product.Description;
            existingProduct.Author = product.Author;
            existingProduct.Price = product.Price;
            existingProduct.Image = product.Image;
            existingProduct.GenreId = product.GenreId;

            _db.Books.Update(existingProduct);
            await _db.SaveChangesAsync();
        }
    }
}
