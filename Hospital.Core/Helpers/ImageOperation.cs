using Hospital.Core.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Hospital.Core.Helpers
{
    public class ImageOperation
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public ImageOperation(IWebHostEnvironment webHostEnvironment, string path)
        {
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagesPath}{path}";
        }

        public async Task<string> SaveImage(IFormFile file)
        {
            string fileName = null!;

            if (file is not null)
            {
                fileName = $"{Guid.NewGuid() + "-" + file.FileName}";

                var path = Path.Combine(_imagesPath, fileName);

                using var stream = File.Create(path);
                await file.CopyToAsync(stream);
            }

            return fileName;
        }
    }
}
