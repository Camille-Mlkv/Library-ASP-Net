 using Newtonsoft.Json;
using System;
using System.Drawing.Printing;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Services.BookService;
using WEB_253502_Melikava.UI.Services.FileService;

namespace WEB_253502_Melikava.UI.Services.API
{
    public class ApiBookService : IBookService
    {
        private HttpClient _httpClient;
        private string _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiBookService> _logger;
        private readonly IFileService _fileService;
        public ApiBookService(HttpClient httpClient,IConfiguration configuration, ILogger<ApiBookService> logger,IFileService fileService)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
            _fileService = fileService;
        }

        public async Task<ResponseData<BookListModel<Book>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Books?");
            if (categoryNormalizedName != null)
            {
                urlString.Append($"&genre={categoryNormalizedName}");
            }
            urlString.Append($"&pageNo={pageNo}&pageSize={_pageSize}");

            var response = await _httpClient.GetAsync(new Uri(urlString.ToString()));

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<BookListModel<Book>>>(_serializerOptions);
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<BookListModel<Book>>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");

            return ResponseData<BookListModel<Book>>.Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");

        }

        public async Task<ResponseData<Book>> CreateProductAsync(Book book, IFormFile? formFile)
        {
            book.Image = "https://localhost:7002/Images/no-image.jpg";
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    book.Image = imageUrl;
                }
            }

            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Books");
            var response = await _httpClient.PostAsJsonAsync(uri, book,_serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data=await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
                return data; 
            }
            _logger.LogError($"-----> object not created. Error:{ response.StatusCode.ToString()}");
             return ResponseData<Book>.Error($"Объект не добавлен. Error: {response.StatusCode.ToString()}");
        }

        public async Task DeleteProductAsync(int id)
        {
            var url = $"{_httpClient.BaseAddress.AbsoluteUri}Books/{id}";
            var response = await _httpClient.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении книги с ID {id}. Статус: {response.StatusCode}");
            }
        }

        public async Task<ResponseData<Book>> GetProductByIdAsync(int id)
        {
            var url = $"{_httpClient.BaseAddress.AbsoluteUri}Books/{id}";
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<ResponseData<Book>>();
                return result;
            }

            throw new Exception($"Не удалось получить книгу с ID {id}. Статус: {response.StatusCode}");
        }

        public async Task UpdateProductAsync(int id, Book product, IFormFile? formFile) 
        {
            if (formFile != null)
            {
                var imageUrl = await _fileService.SaveFileAsync(formFile);
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    var oldImageUrl = product.Image;
                    await _fileService.DeleteFileAsync(oldImageUrl);
                    product.Image = imageUrl;
                    
                }
            }
            var url = $"{_httpClient.BaseAddress.AbsoluteUri}Books/{id}";
            var json = JsonConvert.SerializeObject(product);
            var contentData = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(url, contentData);


            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении книги с ID {id}. Статус: {response.StatusCode}");
            }
        }
    }
}
