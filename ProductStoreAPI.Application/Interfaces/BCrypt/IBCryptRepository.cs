﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Interfaces.BCrypt
{
    public interface IBCryptRepository
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string password);
    }
}
