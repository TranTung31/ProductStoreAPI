using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Core.Entities
{
    public class Status
    {
        [Key]
        public int Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
