using Microsoft.AspNetCore.Http;
using ProductStoreAPI.Application.Interfaces.Catalog.Products;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure.Implement.Catalog.Products
{
    public class ProductImageService : IProductImageService
    {
        public Task<string> UploadSingleImage(IFormFile file)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> UploadMultipleImage(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
