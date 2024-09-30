using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductStoreAPI.Application.DTOs.Catalog.Categories;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using ProductStoreAPI.Application.Interfaces.Catalog.Categories;
using ProductStoreAPI.Core.Entities;
using ProductStoreAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure.Implement.Catalog.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductStoreDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(ProductStoreDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiResult<List<CategoryResponse>>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            var result = new List<CategoryResponse>();

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    var item = _mapper.Map<CategoryResponse>(category);
                    result.Add(item);
                }
            }

            return new ApiSuccessResult<List<CategoryResponse>>(result, "Lấy danh sách danh mục thành công!");
        }

        public async Task<ApiResult<CategoryResponse>> GetCategoryByIdAsync(string id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(int.Parse(id));
                var result = _mapper.Map<CategoryResponse>(category);

                if (category == null)
                {
                    return new ApiErrorResult<CategoryResponse>("Không tìm thấy danh mục!");
                }

                return new ApiSuccessResult<CategoryResponse>(result, "Lấy danh mục theo id thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<CategoryResponse>("Lỗi khi tìm kiếm danh mục theo id: " + ex.Message);
            }
        }

        public async Task<ApiResult<CategoryResponse>> AddAsync(CategoryRequest categoryRequest)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryRequest);
                category.Name = categoryRequest.Name;

                _context.Categories.Add(category);
                await _context.SaveChangesAsync();

                var result = await GetCategoryByIdAsync(category.Id.ToString());
                var categoryResult = result.Data;

                return new ApiSuccessResult<CategoryResponse>(categoryResult, "Thêm danh mục mới thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<CategoryResponse>("Lỗi khi thêm danh mục: " + ex.Message);
            }
        }

        public async Task<ApiResult<CategoryResponse>> UpdateAsync(CategoryRequest categoryRequest)
        {
            try
            {
                var category = await _context.Categories.FindAsync(categoryRequest.Id);

                if (category == null)
                {
                    return new ApiErrorResult<CategoryResponse>("Không tìm thấy danh mục để sửa!");
                }

                category.Name = categoryRequest.Name;
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();

                var result = _mapper.Map<CategoryResponse>(category);

                return new ApiSuccessResult<CategoryResponse>(result, "Sửa danh mục thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<CategoryResponse>("Lỗi khi sửa danh mục: " + ex.Message);
            }
        }

        public async Task<ApiResult<string>> DeleteAsync(string id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(int.Parse(id));

                if (category == null)
                {
                    return new ApiErrorResult<string>("Không tìm thấy danh mục!");
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<string>("", "Xóa danh mục thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("Lỗi khi xóa danh mục: " + ex.Message);
            }
        }
    }
}
