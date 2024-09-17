using Microsoft.AspNetCore.Http;
using ProductStoreAPI.Application.Interfaces.Cloudinary;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ProductStoreAPI.Infrastructure.Implement.CloudinaryImage
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(
                config.Value.CloudName,
                config.Value.ApiKey,
                config.Value.ApiSecret
                );
            _cloudinary = new Cloudinary(account);
        }

        public async Task<string> UploadSingleImage(IFormFile file)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, file.OpenReadStream())
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult.SecureUrl.ToString();
        }

        public async Task<List<string>> UploadMultipleImages(List<IFormFile> files)
        {
            var uploadUrls = new List<string>();

            foreach (var file in files)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };
                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                uploadUrls.Add(uploadResult.SecureUrl.ToString());
            }

            return uploadUrls;
        }
    }
}
