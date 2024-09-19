using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Core.Entities
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        [StringLength(255)]
        public string Address { get; set; } = string.Empty;
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int StatusId { get; set; }
        public Status? Status { get; set; }
        public int PaymentId { get; set; }
        public Payment? Payment { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
