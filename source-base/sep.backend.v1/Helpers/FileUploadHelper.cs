namespace sep.backend.v1.Helpers
{
    public class FileUploadHelper
    {
        private readonly string _baseFolder;

        public FileUploadHelper()
        {
            _baseFolder = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
            if (!Directory.Exists(_baseFolder))
            {
                Directory.CreateDirectory(_baseFolder);
            }
        }

        public async Task<string> UploadFile(IFormFile file, string subFolder = "images")
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Not accept file");

            var uploadFolder = Path.Combine(_baseFolder, subFolder);
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";
           
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("Resources", subFolder, uniqueFileName).Replace("\\", "/");
        }

        public async Task<string> UploadExcelFile(IFormFile file, string subFolder = "excel")
        {
            if (file == null || file.Length == 0 || !Path.GetExtension(file.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Invalid Excel file");

            var uploadFolder = Path.Combine(_baseFolder, subFolder);
            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid().ToString()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return Path.Combine("Resources", subFolder, uniqueFileName).Replace("\\", "/"); 
        }

    }
}
