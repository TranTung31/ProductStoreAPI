using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.DTOs.Catalog.RefreshTokens
{
    public class RefreshTokenRequestDto
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public int UserId { get; set; }
    }
}
