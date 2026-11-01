using Gezo.Application.Interfaces.Services;
using Gezo.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gezo.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;

        public ImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async  Task<string> UploadAsync(IFormFile file)
        {
            var uploadsFolder = Path.Combine( _environment.WebRootPath,"uploads","products");

            Directory.CreateDirectory(uploadsFolder);
            if (file == null || file.Length == 0)
                throw new BadRequestCustomException("Invalid image file.");
            const long maxFileSize = 5 * 1024 * 1024;

            if (file.Length > maxFileSize)
                throw new BadRequestCustomException("Image size exceeds the allowed limit.");

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new BadRequestCustomException("Unsupported image extension.");
            var fileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(stream);

            return $"/uploads/products/{fileName}";
        }
        public Task DeleteAsync(string imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return Task.CompletedTask;

            var fullPath = Path.Combine(
                _environment.WebRootPath,
                imagePath.TrimStart('/')
            );

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }

            return Task.CompletedTask;
        }

      
    }
}
