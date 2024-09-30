using ProductStoreAPI.Application.DTOs.Catalog.Orders;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Interfaces.Catalog.Orders
{
    public interface IOrderRepository
    {
        Task<ApiResult<List<OrderResponseDto>>> GetOrdersAsync(string? search, int page);
        Task<ApiResult<OrderResponseDto>> GetOrderByIdAsync(int id);
        Task<ApiResult<OrderResponseDto>> AddAsync(OrderRequestDto orderRequestDto);
        Task<ApiResult<string>> UpdateAsync(int id, OrderUpdateRequestDto orderUpdateRequestDto);
        Task<ApiResult<string>> DeleteAsync(int id);
    }
}
