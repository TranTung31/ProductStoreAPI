using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductStoreAPI.Application.DTOs.Catalog.Orders;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using ProductStoreAPI.Application.Interfaces.Catalog.Orders;
using ProductStoreAPI.Application.Interfaces.Catalog.Products;
using ProductStoreAPI.Core.Entities;
using ProductStoreAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure.Implement.Catalog.Orders
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ProductStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly IProductRepository _productRepository;
        private static int PAGE_SIZE { get; set; } = 3;

        public OrderRepository(ProductStoreDbContext context, IMapper mapper, IProductRepository productRepository)
        {
            _context = context;
            _mapper = mapper;
            _productRepository = productRepository;
        }

        public async Task<ApiResult<List<OrderResponseDto>>> GetOrdersAsync(string? search, int page)
        {
            try
            {
                var orders = _context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.Status)
                    .Include(o => o.Payment)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(search))
                {
                    orders = orders.Where(o => o.CustomerName.ToLower().Contains(search.ToLower()));
                }

                orders = orders.Where(o => o.IsDeleted == false).Skip((page - 1) * PAGE_SIZE).Take(PAGE_SIZE);

                var ordersResult = await orders.ToListAsync();
                var result = new List<OrderResponseDto>();

                foreach (var item in ordersResult)
                {
                    var order = _mapper.Map<OrderResponseDto>(item);
                    order.PaymentMethod = item.Payment.PaymentMethod;

                    foreach (var orderItem in order.OrderItems)
                    {
                        var existingProduct = await _productRepository.GetProductByIdAsync(orderItem.ProductId.ToString());

                        if (existingProduct.Data == null)
                        {
                            return new ApiErrorResult<List<OrderResponseDto>>($"Không tìm thấy sản phẩm có id {orderItem.ProductId}!");
                        }

                        var product = existingProduct.Data;
                        orderItem.ProductName = product.Name;
                        orderItem.ProductDescription = product.Description;
                        orderItem.ProductImagePath = product.ImagePath;
                    }

                    result.Add(order);
                }

                return new ApiSuccessResult<List<OrderResponseDto>>(result, "Lấy danh sách đơn hàng thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<OrderResponseDto>>("Lỗi khi lấy danh sách đơn hàng! " + ex.Message);
            }
        }

        public async Task<ApiResult<OrderResponseDto>> GetOrderByIdAsync(int id)
        {
            try
            {
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                    .Include(o => o.Status)
                    .Include(o => o.Payment)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                {
                    return new ApiErrorResult<OrderResponseDto>($"Không tìm thấy đơn hàng có id {id}!");
                }

                var result = _mapper.Map<OrderResponseDto>(existingOrder);
                result.PaymentMethod = existingOrder.Payment.PaymentMethod;

                foreach (var item in result.OrderItems)
                {
                    var existingProduct = await _productRepository.GetProductByIdAsync(item.ProductId.ToString());

                    if (existingProduct.Data == null)
                    {
                        return new ApiErrorResult<OrderResponseDto>($"Không tìm thấy sản phẩm có id {item.ProductId}!");
                    }

                    var product = existingProduct.Data;
                    item.ProductName = product.Name;
                    item.ProductDescription = product.Description;
                    item.ProductImagePath = product.ImagePath;
                }

                return new ApiSuccessResult<OrderResponseDto>(result, $"Lấy đơn hàng có id {id} thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<OrderResponseDto>("Lỗi khi lấy đơn hàng theo id! " + ex.Message);
            }
        }

        public async Task<ApiResult<OrderResponseDto>> AddAsync(OrderRequestDto orderRequestDto)
        {
            //using var transaction = await _context.Database.BeginTransactionAsync();
            var order = new Order
            {
                CustomerName = orderRequestDto.CustomerName,
                Address = orderRequestDto.Address,
                PhoneNumber = orderRequestDto.PhoneNumber,
                OrderDate = DateTime.UtcNow, // Ngày đặt hàng
                TotalAmount = 0, // Tổng tiền sẽ được tính toán lại bên dưới
                IsDeleted = false,
                StatusId = orderRequestDto.StatusId,
                PaymentId = orderRequestDto.PaymentId,
            };

            foreach (var item in orderRequestDto.OrderItems)
            {
                var productResult = await _productRepository.GetProductByIdAsync(item.ProductId.ToString());
                var existingProduct = _context.Products.Local.FirstOrDefault(p => p.Id == item.ProductId);

                if (productResult.Data == null || existingProduct == null)
                {
                    return new ApiErrorResult<OrderResponseDto>("Không tìm thấy sản phẩm!");
                }

                var product = productResult.Data;

                if (product.Stock < item.Quantity)
                {
                    return new ApiErrorResult<OrderResponseDto>($"Số lượng đặt hàng của sản phẩm {product.Name} không được phép quá {product.Stock}!");
                }

                // Trừ đi số lượng sản phẩm trong kho
                product.Stock -= item.Quantity;
                _context.Entry(existingProduct).CurrentValues.SetValues(product);

                order.TotalAmount += item.Quantity * product.Price;

                var orderItem = _mapper.Map<OrderItem>(item);
                orderItem.Price = product.Price;

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return new ApiSuccessResult<OrderResponseDto>();
        }

        public async Task<ApiResult<string>> UpdateAsync(int id, OrderUpdateRequestDto orderUpdateRequestDto)
        {
            try
            {
                var existingOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                {
                    return new ApiErrorResult<string>($"Đơn hàng có id {id} không tồn tại!");
                }

                existingOrder.StatusId = orderUpdateRequestDto.StatusId;

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<string>("", $"Cập nhật đơn hàng có id {id} thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("Lỗi khi cập nhật đơn hàng! " + ex.Message);
            }
        }

        public async Task<ApiResult<string>> DeleteAsync(int id)
        {
            try
            {
                var existingOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                    .FirstOrDefaultAsync(o => o.Id == id);

                if (existingOrder == null)
                {
                    return new ApiErrorResult<string>($"Đơn hàng có id {id} không tồn tại!");
                }

                existingOrder.IsDeleted = true;

                foreach (var item in existingOrder.OrderItems)
                {
                    var productResult = await _productRepository.GetProductByIdAsync(item.ProductId.ToString());
                    var existingProduct = _context.Products.Local.FirstOrDefault(p => p.Id == item.ProductId);

                    if (productResult.Data == null || existingProduct == null)
                    {
                        return new ApiErrorResult<string>($"Sản phẩm có id {item.ProductId} không tồn tại!");
                    }

                    var product = productResult.Data;
                    product.Stock += item.Quantity;

                    _context.Entry(existingProduct).CurrentValues.SetValues(product);
                }

                _context.Orders.Update(existingOrder);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<string>("", $"Xóa đơn hàng có id {id} thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("Lỗi khi xóa đơn hàng! " + ex.Message);
            }
        }
    }
}
