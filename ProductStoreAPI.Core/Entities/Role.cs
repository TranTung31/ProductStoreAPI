using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Core.Entities
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string Description { get; set; } = string.Empty;

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
