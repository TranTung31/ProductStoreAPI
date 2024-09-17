using Microsoft.AspNetCore.Mvc;
using ProductStoreAPI.Application.DTOs.Catalog.Products;
using ProductStoreAPI.Application.Interfaces.Catalog.Products;

namespace ProductStoreAPI.WebAPI.Controllers
{
    [Route("/api/v1/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts(string? search, int page = 1)
        {
            var result = await _productService.GetProductsAsync(search, page);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var result = await _productService.GetProductByIdAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductRequestDto productRequestDto)
        {
            var result = await _productService.AddAsync(productRequestDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductRequestDto productRequestDto)
        {
            var result = await _productService.UpdateAsync(productRequestDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await _productService.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
