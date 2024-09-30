using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.DTOs.Catalog.Users
{
    public class UserRequestRefreshTokenDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
