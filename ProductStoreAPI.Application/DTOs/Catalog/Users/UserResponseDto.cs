using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.DTOs.Catalog.Users
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Avatar { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
    }
}
