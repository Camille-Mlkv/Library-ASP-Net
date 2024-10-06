using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WEB_253502_Melikava.API.Data;
using WEB_253502_Melikava.API.Services.BookService;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;

namespace WEB_253502_Melikava.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IBookService _bookService; 

        public BooksController(AppDbContext context,IBookService bookService)
        {
            _context = context;
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseData<List<Book>>>> GetBooks(string? genre,int pageNo = 1,int pageSize = 3)
        {
            return Ok(await _bookService.GetProductListAsync(genre,pageNo,pageSize));
        }

        [HttpPost]
        public async Task<ActionResult<Book>> PostBook(Book book)
        {
            return Ok(await _bookService.CreateProductAsync(book));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                await _bookService.DeleteProductAsync(id);
                return Ok($"Book with ID {id} was deleted successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            return Ok(await _bookService.GetProductByIdAsync(id));
        }

        [HttpPost]
        [Route("{id:int}/upload-image")]
        public async Task<IActionResult> UploadImage(int id, IFormFile formFile)
        {
            return Ok(await _bookService.SaveImageAsync(id, formFile));
            
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> PutBook(int id, Book book)
        {
            try
            {
                await _bookService.UpdateProductAsync(id,book);
                return Ok($"Book with ID {id} was updated successfully.");
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

    }
}
