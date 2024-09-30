using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Core.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [StringLength(255)]
        public string Address { get; set; } = string.Empty;

        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(255)]
        public string Avatar { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public bool IsDeleted { get; set; } = false;

        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
