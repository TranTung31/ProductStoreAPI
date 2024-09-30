using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Interfaces.Cloudinary
{
    public interface ICloudinaryRepository
    {
        Task<string> UploadSingleImage(IFormFile file);
        Task<List<string>> UploadMultipleImages(List<IFormFile> files);
    }
}
