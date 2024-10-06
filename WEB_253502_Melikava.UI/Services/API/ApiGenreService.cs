using System.Data.SqlTypes;
using System.Text.Json;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using WEB_253502_Melikava.UI.Services.GenreService;

namespace WEB_253502_Melikava.UI.Services.API
{
    public class ApiGenreService : IGenreService
    {
        private HttpClient _httpClient;
        private JsonSerializerOptions _serializerOptions;
        private ILogger<ApiBookService> _logger;

        public ApiGenreService(HttpClient httpClient, ILogger<ApiBookService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _serializerOptions = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }


        //https://localhost:7002/api/Genres
        public async Task<ResponseData<List<Genre>>> GetGenreListAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Genres");
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    return await response.Content.ReadFromJsonAsync<ResponseData<List<Genre>>>(_serializerOptions);
                }
                catch (Newtonsoft.Json.JsonException ex)
                {
                    _logger.LogError($"-----> Ошибка: {ex.Message}");
                    return ResponseData<List<Genre>>.Error($"Ошибка: {ex.Message}");
                }
            }
            _logger.LogError($"-----> Данные не получены от сервера. Error: {response.StatusCode.ToString()}");

            return ResponseData<List<Genre>>.Error($"Данные не получены от сервера. Error: {response.StatusCode.ToString()}");
        }
    }
}
