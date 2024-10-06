
using Microsoft.AspNetCore.Http;

namespace WEB_253502_Melikava.UI.Services.FileService
{
    public class ApiFileService : IFileService
    {
        private readonly HttpClient _httpClient;
        public ApiFileService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> SaveFileAsync(IFormFile formFile)
        {
            // Создать объект запроса
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post
            };
            // Сформировать случайное имя файла, сохранив расширение
            var extension = Path.GetExtension(formFile.FileName);
            var newName = Path.ChangeExtension(Path.GetRandomFileName(), extension);
            // Создать контент, содержащий поток загруженного файла
            var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(formFile.OpenReadStream());
            content.Add(streamContent, "file", newName);
            // Поместить контент в запрос
            request.Content = content;
            // Отправить запрос к API
            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Вернуть полученный Url сохраненного файла
                return await response.Content.ReadAsStringAsync();
            }
            return String.Empty;
        }
        public async Task DeleteFileAsync(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("File name must not be empty", nameof(fileName));
            }
            //'https://localhost:7002/api/Files'
            var onlyName = Path.GetFileName(new Uri(fileName).LocalPath);
            var requestUrl = $"https://localhost:7002/api/Files?fileName={Uri.EscapeDataString(onlyName)}";
            var response = await _httpClient.DeleteAsync(requestUrl);

            if (!response.IsSuccessStatusCode)
            {
                // Обработка ошибок
                throw new Exception($"Failed to delete the file. Status code: {response.StatusCode}, Reason: {response.ReasonPhrase}");
            }
        }
    }
}
