using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Controllers
{
    [Route("file")]
    public class FileController : ControllerBase
    {
        [HttpGet]
        [ResponseCache(Duration = 1200, VaryByQueryKeys = new[] { "fileName" })]
        public async Task<ActionResult> GetFile([FromQuery] string fileName)
        {
            var rootPath = await Task.FromResult(Directory.GetCurrentDirectory());

            var filePath = $"{rootPath}/PrivateFiles/{fileName}";

            var isFileExist = await Task.FromResult(System.IO.File.Exists(filePath));

            if (!isFileExist)
            {
                throw new NotFoundException("File not found!");
            }

            var contentProvider = new FileExtensionContentTypeProvider();
            await Task.FromResult(contentProvider.TryGetContentType(filePath, out string contentType));

            var fileContents = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        public async Task<ActionResult> UploadFile([FromForm] IFormFile formFile)
        {
            if (formFile != null && formFile.Length > 0)
            {
                var rootPath = await Task.FromResult(Directory.GetCurrentDirectory());
                var fileName = await Task.FromResult(formFile.FileName);
                var fullPath = await Task.FromResult($"{rootPath}/PrivateFiles/{fileName}");

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await formFile.CopyToAsync(stream);
                }

                return Ok();
            }

            return BadRequest();
        }
    }
}