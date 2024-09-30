using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Core.Entities
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Token { get; set; } = string.Empty;

        public DateTime ExpiryDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public User? User { get; set; }

        public bool IsRevoked { get; set; } = false;
    }
}
