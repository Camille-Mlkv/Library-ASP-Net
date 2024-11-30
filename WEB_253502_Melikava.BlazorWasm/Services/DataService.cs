using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data.SqlTypes;
using System.Net.Http.Json;
using System.Text;
using WEB_253502_Melikava.Domain.Entities;
using WEB_253502_Melikava.Domain.Models;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Net.Http.Headers;

namespace WEB_253502_Melikava.BlazorWasm.Services
{
    public class DataService : IDataService
    {
        public List<Genre> Genres { get; set; }
        public List<Book> Books { get; set; }
        public bool Success { get; set; }
        public string? ErrorMessage { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public Genre? SelectedGenre { get; set; }
        public event Action DataLoaded;

        private HttpClient _httpClient;
        private string _pageSize;
        private IAccessTokenProvider _tokenProvider;
        public DataService(IHttpClientFactory factory, IConfiguration configuration,IAccessTokenProvider tokenPorvider)
        {
            _httpClient = factory.CreateClient("ApiClient");
            _pageSize = configuration.GetSection("ItemsPerPage").Value;
            _tokenProvider = tokenPorvider;
        }

        private async Task GetToken()
        {
            var tokenRequest = await _tokenProvider.RequestAccessToken();
            if (tokenRequest.TryGetToken(out var token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.Value);
            }
        }

        public async Task GetBookListAsync(string? genreNormalizedName, int pageNo = 1)
        {

            //https://localhost:7002/api/Books?genre=detectives&pageNo=1&pageSize=3
            //https://localhost:7002/api/Books?pageNo=1&pageSize=3

            await GetToken();

            var route = new StringBuilder("Books");
            // Проверяем, добавлять ли параметр genre
            if (SelectedGenre is not null)
            {
                route.Append($"?genre={SelectedGenre.NormalizedName}");
            }

            List<KeyValuePair<string, string>> queryData = new()
            {
                KeyValuePair.Create("pageNo", pageNo.ToString()),
                KeyValuePair.Create("pageSize", _pageSize)
            };

            var queryString = QueryString.Create(queryData);

            if (SelectedGenre is not null)
            {
                route.Append('&');
                route.Append(queryString.ToUriComponent().TrimStart('?'));
            }
            else
            {
                route.Append(queryString);
            }

            var response = await _httpClient.GetAsync(route.ToString());
            //var response = await _httpClient.GetAsync("https://localhost:7002/api/Books?genre=detectives&pageNo=1&pageSize=3");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<BookListModel<Book>>>();
                Books = data?.Data?.Items;
                TotalPages = data?.Data?.TotalPages ?? 0;
                CurrentPage = data?.Data?.CurrentPage ?? 0;

                DataLoaded?.Invoke();
                
            }
        }

        public async Task GetGenreListAsync()
        {
            var uri = new Uri(_httpClient.BaseAddress.AbsoluteUri + "Genres");
            var response = await _httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<ResponseData<List<Genre>>>();
                if (data?.Data != null)
                {
                    Genres = data.Data;
                }
            }
            else
            {
                Console.WriteLine($"Failed to fetch genres: {response.StatusCode}");
            }
        }

        
    }
}
