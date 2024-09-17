using ProductStoreAPI.Application.DTOs.Catalog.Products;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Interfaces.Catalog.Products
{
    public interface IProductService
    {
        Task<ApiResult<List<ProductResponseDto>>> GetProductsAsync(string? search, int page);
        Task<ApiResult<ProductResponseDto>> GetProductByIdAsync(string id);
        Task<ApiResult<ProductResponseDto>> AddAsync(ProductRequestDto productRequestDto);
        Task<ApiResult<ProductResponseDto>> UpdateAsync(ProductRequestDto productRequestDto);
        Task<ApiResult<string>> DeleteAsync(string id);
    }
}
