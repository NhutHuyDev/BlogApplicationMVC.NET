using Microsoft.AspNetCore.Hosting;

namespace BlogApplication.MVC.Utilities
{
    public class FileHandler
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FileHandler(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public string UploadImage(IFormFile file)
        {
            var folderPath = Path.Combine(_webHostEnvironment.WebRootPath, "thumbnails");
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using (FileStream fileStream = File.Create(filePath))
            {
                file.CopyTo(fileStream);
            }
            return uniqueFileName;
        }
    }
}
