using Microsoft.AspNetCore.Mvc;
using ProductStoreAPI.Application.DTOs.Catalog.Categories;
using ProductStoreAPI.Application.Interfaces;

namespace ProductStoreAPI.WebAPI.Controllers
{
    [ApiController]
    [Route("/api/v1/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoryService.GetCategoriesAsync();
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var result = await _categoryService.GetCategoryByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CategoryRequest categoryRequest)
        {
            var result = await _categoryService.AddAsync(categoryRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(CategoryRequest categoryRequest)
        {
            var result = await _categoryService.UpdateAsync(categoryRequest);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
