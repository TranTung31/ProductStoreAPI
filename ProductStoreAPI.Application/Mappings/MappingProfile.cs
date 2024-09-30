using AutoMapper;
using ProductStoreAPI.Application.DTOs.Catalog.Categories;
using ProductStoreAPI.Application.DTOs.Catalog.Orders;
using ProductStoreAPI.Application.DTOs.Catalog.Products;
using ProductStoreAPI.Application.DTOs.Catalog.RefreshTokens;
using ProductStoreAPI.Application.DTOs.Catalog.Users;
using ProductStoreAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Category
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryRequest>().ReverseMap();

            // Product
            CreateMap<Product, ProductResponseDto>().ReverseMap();
            CreateMap<Product, ProductRequestDto>().ReverseMap();

            // Order
            CreateMap<Order, OrderResponseDto>().ReverseMap();
            CreateMap<Order, OrderRequestDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemResponseDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemRequestDto>().ReverseMap();

            // User
            CreateMap<User, UserRequestSignUpDto>().ReverseMap();
            CreateMap<User, UserResponseDto>().ReverseMap();

            // RefreshToken
            CreateMap<RefreshToken, RefreshTokenRequestDto>().ReverseMap();
        }
    }
}
