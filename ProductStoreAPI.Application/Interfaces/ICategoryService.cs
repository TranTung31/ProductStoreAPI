using ProductStoreAPI.Application.DTOs.Catalog.Categories;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResult<List<CategoryResponse>>> GetCategoriesAsync();
        Task<ApiResult<CategoryResponse>> GetCategoryByIdAsync(string id);
        Task<ApiResult<CategoryResponse>> AddAsync(CategoryRequest categoryRequest);
        Task<ApiResult<CategoryResponse>> UpdateAsync(CategoryRequest categoryRequest);
        Task<ApiResult<string>> DeleteAsync(string id);
    }
}
