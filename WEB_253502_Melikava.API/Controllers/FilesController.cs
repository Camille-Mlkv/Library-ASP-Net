using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WEB_253502_Melikava.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // Путь к папке wwwroot/Images
        private readonly string _imagePath;
        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file is null)
            {
                return BadRequest();
            }
            var filePath = Path.Combine(_imagePath, file.FileName);
            var fileInfo = new FileInfo(filePath);
            // если такой файл существует, удалить его
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            // скопировать файл в поток
            using var fileStream = fileInfo.Create();
            await file.CopyToAsync(fileStream);
            // получить Url файла
            var host = HttpContext.Request.Host;
            var fileUrl = $"https://{host}/Images/{file.FileName}";
            return Ok(fileUrl);

        }

        [HttpDelete]
        public IActionResult DeleteFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name is not provided.");
            }

            var filePath = Path.Combine(_imagePath, fileName);
            var fileInfo = new FileInfo(filePath);

            // Check if the file exists
            if (!fileInfo.Exists)
            {
                return NotFound($"File '{fileName}' not found.");
            }

            try
            {
                // Delete the file
                fileInfo.Delete();
                return Ok($"File '{fileName}' successfully deleted.");
            }
            catch (Exception ex)
            {
                // Log the exception if needed and return a server error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
