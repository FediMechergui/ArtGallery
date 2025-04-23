using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace ArtGallery.Services
{
    // Interface pour la gestion des fichiers (upload, suppression).
    public interface IFileService
    {
        // Téléverse un fichier de manière asynchrone dans un sous-dossier.
        Task<string> UploadFileAsync(IFormFile file, string subDirectory);
        // Supprime un fichier selon son chemin relatif.
        void DeleteFile(string filePath);
    }

    // Service concret pour la gestion des fichiers (upload, suppression).
    public class FileService : IFileService
    {
        // Configuration de l'application.
        private readonly IConfiguration _configuration;
        // Chemin de base pour le stockage des fichiers uploadés.
        private readonly string _basePath;

        // Constructeur du service de fichiers.
        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        }

        // Téléverse un fichier dans un sous-dossier et retourne le chemin relatif.
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

        // Supprime un fichier du stockage local selon son chemin relatif.
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