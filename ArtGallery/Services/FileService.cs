using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ArtGallery.Services
{
    public interface IFileService
    {
        Task<string> UploadFileAsync(IFormFile file, string subDirectory);
        void DeleteFile(string filePath);
    }

    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        private readonly string _basePath;

        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subDirectory)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");

            var directory = Path.Combine(_basePath, subDirectory);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(directory, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return Path.Combine("uploads", subDirectory, fileName).Replace("\\", "/");
        }

        public void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
                return;

            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
} 