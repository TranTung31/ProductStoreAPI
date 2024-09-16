using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Interfaces.Catalog.Products
{
    public interface IProductImageService
    {
        Task<string> UploadSingleImage(IFormFile file);
        Task<List<string>> UploadMultipleImage(List<IFormFile> files);
    }
}
