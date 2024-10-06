using Newtonsoft.Json;
using System;
using System.Drawing.Printing;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Services.BookService;

namespace WEB_253502_Melikava.UI.Services.API
{
    public class ApiBookService : IBookService
    {
        private HttpClient _httpClient;
        private string _pageSize;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiBookService> _logger;
        public ApiBookService(HttpClient httpClient,IConfiguration configuration, ILogger<ApiBookService> logger)
        {
            _httpClient = httpClient;
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            _logger = logger;
        }

        //https://localhost:7002/api/Books?pageNo=1&pageSize=3
        //https://localhost:7002/api/Books?genre=classic&pageNo=1&pageSize=3
        public async Task<ResponseData<BookListModel<Book>>> GetProductListAsync(string? categoryNormalizedName, int pageNo = 1)
        {
            // подготовка URL запроса
            var urlString = new StringBuilder($"{_httpClient.BaseAddress.AbsoluteUri}Books?");
            // добавить категорию в маршрут
            if (categoryNormalizedName != null)
            {
                urlString.Append($"&genre={categoryNormalizedName}");
            }
            urlString.Append($"&pageNo={pageNo}&pageSize={_pageSize}");

            // отправить запрос к API
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

        //https://localhost:7002/api/Books
        public async Task<ResponseData<Book>> CreateProductAsync(Book book, IFormFile? formFile) //Testing needed in LR5
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Dishes");
            var response = await _httpClient.PostAsJsonAsync(uri, book,_serializerOptions);
            if (response.IsSuccessStatusCode)
            {
                var data=await response.Content.ReadFromJsonAsync<ResponseData<Book>>(_serializerOptions);
                return data; 
            }
            _logger.LogError($"-----> object not created. Error:{ response.StatusCode.ToString()}");
             return ResponseData<Book>.Error($"Объект не добавлен. Error: {response.StatusCode.ToString()}");
        }

        //https://localhost:7002/api/Books/17
        public async Task DeleteProductAsync(int id) //Testing needed in LR5
        {
            var url = $"{_httpClient.BaseAddress.AbsoluteUri}Books/{id}";
            var response = await _httpClient.DeleteAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при удалении книги с ID {id}. Статус: {response.StatusCode}");
            }
        }

        //https://localhost:7002/api/Books/17
        public async Task<ResponseData<Book>> GetProductByIdAsync(int id) //Testing needed in LR5
        {
            var url = $"{_httpClient.BaseAddress.AbsoluteUri}Books/{id}";

            // Выполняем GET запрос
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                // Возвращаем данные о книге
                var result = await response.Content.ReadFromJsonAsync<ResponseData<Book>>();
                return result;
            }

            throw new Exception($"Не удалось получить книгу с ID {id}. Статус: {response.StatusCode}");
        }

        //https://localhost:7002/api/Books/17
        public async Task UpdateProductAsync(int id, Book product, IFormFile? formFile) //Testing needed in LR5
        {
            var url = $"{_httpClient.BaseAddress.AbsoluteUri}Books/{id}";

            // Подготовка содержимого для отправки в запросе
            var content = new MultipartFormDataContent();

            // Сериализация объекта книги в JSON и добавление в содержимое запроса
            var productContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            content.Add(productContent, "product");

            // Если есть файл, добавляем его в содержимое запроса
            if (formFile != null)
            {
                var fileStream = formFile.OpenReadStream();
                content.Add(new StreamContent(fileStream), "file", formFile.FileName);
            }

            // Выполняем PUT запрос для обновления продукта
            var response = await _httpClient.PutAsync(url, content);

            // Проверяем результат
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Ошибка при обновлении книги с ID {id}. Статус: {response.StatusCode}");
            }
        }
    }
}
