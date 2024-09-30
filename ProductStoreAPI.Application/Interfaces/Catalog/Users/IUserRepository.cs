using ProductStoreAPI.Application.DTOs.Catalog.Users;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Application.Interfaces.Catalog.Users
{
    public interface IUserRepository
    {
        Task<ApiResult<string>> SignUpAsync(UserRequestSignUpDto userRequestSignUpDto);
        Task<ApiResult<UserResponseSignInDto>> SignInAsync(UserRequestSignInDto userRequestSignInDto);
        Task<ApiResult<UserResponseRefreshTokenDto>> RefreshTokenAsync(UserRequestRefreshTokenDto userRequestRefreshTokenDto);
    }
}
