using ProductStoreAPI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.DTOs.Catalog.Orders
{
    public class OrderRequestDto
    {
        public string CustomerName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int StatusId { get; set; }
        public int PaymentId { get; set; }
        public ICollection<OrderItemRequestDto> OrderItems { get; set; } = new List<OrderItemRequestDto>();
    }
}
