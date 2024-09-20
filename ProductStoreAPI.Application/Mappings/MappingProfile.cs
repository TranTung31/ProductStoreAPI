using AutoMapper;
using ProductStoreAPI.Application.DTOs.Catalog.Categories;
using ProductStoreAPI.Application.DTOs.Catalog.Orders;
using ProductStoreAPI.Application.DTOs.Catalog.Products;
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
            CreateMap<Category, CategoryResponse>().ReverseMap();
            CreateMap<Category, CategoryRequest>().ReverseMap();

            CreateMap<Product, ProductResponseDto>().ReverseMap();
            CreateMap<Product, ProductRequestDto>().ReverseMap();

            CreateMap<Order, OrderResponseDto>().ReverseMap();
            CreateMap<Order, OrderRequestDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemResponseDto>().ReverseMap();
            CreateMap<OrderItem, OrderItemRequestDto>().ReverseMap();
        }
    }
}
