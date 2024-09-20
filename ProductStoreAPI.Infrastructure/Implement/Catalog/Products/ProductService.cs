using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductStoreAPI.Application.DTOs.Catalog.Products;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using ProductStoreAPI.Application.Interfaces.Catalog.Products;
using ProductStoreAPI.Application.Interfaces.Cloudinary;
using ProductStoreAPI.Core.Entities;
using ProductStoreAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure.Implement.Catalog.Products
{
    public class ProductService : IProductService
    {
        private readonly ProductStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryService _cloudinaryService;
        private static int PAGE_SIZE { get; set; } = 3;

        public ProductService(ProductStoreDbContext context, IMapper mapper, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _mapper = mapper;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<ApiResult<List<ProductResponseDto>>> GetProductsAsync(string? search, int page)
        {
            try
            {
                // Phân trang
                var products = _context.Products.Include(p => p.Category).AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    products = products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
                }

                products = products.Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

                // Lấy tổng số lượng sản phẩm
                var productTotal = _context.Products.Count();

                var productsResult = await products.ToListAsync();
                var result = new List<ProductResponseDto>();

                foreach (var product in productsResult)
                {
                    var item = _mapper.Map<ProductResponseDto>(product);
                    result.Add(item);
                }

                return new ApiSuccessResult<List<ProductResponseDto>>(result, "Lấy danh sách sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<ProductResponseDto>>("Lỗi khi lấy danh sách sản phẩm: " + ex.Message);
            }
        }

        public async Task<ApiResult<ProductResponseDto>> GetProductByIdAsync(string id)
        {
            try
            {
                var product = await _context.Products.FindAsync(int.Parse(id));

                if (product == null)
                {
                    return new ApiErrorResult<ProductResponseDto>("Không tìm thấy sản phẩm!");
                }

                var result = _mapper.Map<ProductResponseDto>(product);
                var query = from productItem in _context.Products
                            join categoryItem in _context.Categories on productItem.CategoryId equals categoryItem.Id
                            select new
                            {
                                CategoryId = categoryItem.Id,
                                CategoryName = categoryItem.Name,
                            };
                var categoryList = await query.ToListAsync();
                var categoryResult = await query.FirstOrDefaultAsync(x => x.CategoryId == product.CategoryId);

                if (categoryResult != null)
                {
                    result.CategoryName = categoryResult.CategoryName;
                }

                return new ApiSuccessResult<ProductResponseDto>(result, "Lấy sản phẩm theo id thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<ProductResponseDto>("Lỗi khi lấy sản phẩm theo id: " + ex.Message);
            }
        }

        public async Task<ApiResult<ProductResponseDto>> AddAsync(ProductRequestDto productRequestDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productRequestDto);
                product.Name = productRequestDto.Name;
                product.Description = productRequestDto.Description;
                product.Price = productRequestDto.Price;

                if (productRequestDto.ImagePath != null)
                {
                    product.ImagePath = await _cloudinaryService.UploadSingleImage(productRequestDto.ImagePath);
                }

                product.Stock = productRequestDto.Stock;
                product.CategoryId = productRequestDto.CategoryId;

                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                var result = await GetProductByIdAsync(product.Id.ToString());
                var productResult = result.Data;

                return new ApiSuccessResult<ProductResponseDto>(productResult, "Thêm sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<ProductResponseDto>("Lỗi khi thêm sản phẩm: " + ex.Message);
            }
        }

        public async Task<ApiResult<ProductResponseDto>> UpdateAsync(ProductRequestDto productRequestDto)
        {
            try
            {
                var product = await _context.Products.FindAsync(productRequestDto.Id);

                if (product == null)
                {
                    return new ApiErrorResult<ProductResponseDto>("Không tìm thấy sản phẩm!");
                }

                product.Name = productRequestDto.Name;
                product.Description = productRequestDto.Description;
                product.Price = productRequestDto.Price;

                if (productRequestDto.ImagePath != null)
                {
                    product.ImagePath = await _cloudinaryService.UploadSingleImage(productRequestDto.ImagePath);
                }

                product.Stock = productRequestDto.Stock;
                product.CategoryId = productRequestDto.CategoryId;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<ProductResponseDto>(product);
                return new ApiSuccessResult<ProductResponseDto>(result, "Sửa sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<ProductResponseDto>("Lỗi khi sửa sản phẩm: " + ex.Message);
            }
        }

        public async Task<ApiResult<string>> DeleteAsync(string id)
        {
            try
            {
                var product = await _context.Products.FindAsync(int.Parse(id));

                if (product == null)
                {
                    return new ApiErrorResult<string>("Không tìm thấy sản phẩm!");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<string>("", "Xóa sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("Lỗi khi xóa sản phẩm: " + ex.Message);
            }
        }
    }
}
