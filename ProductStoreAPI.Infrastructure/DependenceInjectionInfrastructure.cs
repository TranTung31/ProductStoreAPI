using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProductStoreAPI.Application.Interfaces.BCrypt;
using ProductStoreAPI.Application.Interfaces.Catalog.Categories;
using ProductStoreAPI.Application.Interfaces.Catalog.Orders;
using ProductStoreAPI.Application.Interfaces.Catalog.Products;
using ProductStoreAPI.Application.Interfaces.Catalog.Users;
using ProductStoreAPI.Application.Interfaces.Cloudinary;
using ProductStoreAPI.Infrastructure.Implement.BCryptPassword;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Categories;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Orders;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Products;
using ProductStoreAPI.Infrastructure.Implement.Catalog.Users;
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
            services.AddScoped<ICloudinaryRepository, CloudinaryRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IBCryptRepository, BCryptRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"]))
                };
            });

            return services;
        }
    }
}
