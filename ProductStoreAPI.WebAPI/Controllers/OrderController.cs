using Microsoft.AspNetCore.Mvc;
using ProductStoreAPI.Application.DTOs.Catalog.Orders;
using ProductStoreAPI.Application.Interfaces.Catalog.Orders;

namespace ProductStoreAPI.WebAPI.Controllers
{
    [Route("/api/v1/order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderService)
        {
            _orderRepository = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders(string? search, int page = 1)
        {
            var result = await _orderRepository.GetOrdersAsync(search, page);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(string id)
        {
            var result = await _orderRepository.GetOrderByIdAsync(int.Parse(id));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder(OrderRequestDto orderRequestDto)
        {
            var result = await _orderRepository.AddAsync(orderRequestDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromBody] OrderUpdateRequestDto orderUpdateRequestDto)
        {
            var result = await _orderRepository.UpdateAsync(int.Parse(id), orderUpdateRequestDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(string id)
        {
            var result = await _orderRepository.DeleteAsync(int.Parse(id));
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
