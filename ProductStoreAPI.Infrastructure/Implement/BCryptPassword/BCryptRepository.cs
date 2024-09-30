using ProductStoreAPI.Application.Interfaces.BCrypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure.Implement.BCryptPassword
{
    public class BCryptRepository : IBCryptRepository
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
