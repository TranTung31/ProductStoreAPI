using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductStoreAPI.Application.Interfaces;
using ProductStoreAPI.Application.Interfaces.Catalog.Orders;
using ProductStoreAPI.Application.Interfaces.Catalog.Products;
using ProductStoreAPI.Application.Interfaces.Cloudinary;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Categories;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Orders;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Products;
using ProductStoreAPI.Infrastructure.Implement.CloudinaryImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure
{
    public static class DependenceInjectionInfrastructure
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var cloudinarySettings = configuration.GetSection("CloudinarySettings");
            services.Configure<CloudinarySettings>(cloudinarySettings);
            services.AddScoped<ICloudinaryService, CloudinaryService>();
            services.AddScoped<ICategoryService, CategorySevice>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
