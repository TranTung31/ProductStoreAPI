using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProductStoreAPI.Application.DTOs.Catalog.RefreshTokens;
using ProductStoreAPI.Application.DTOs.Catalog.Users;
using ProductStoreAPI.Application.DTOs.Common.ResponseNotification;
using ProductStoreAPI.Application.Interfaces.BCrypt;
using ProductStoreAPI.Application.Interfaces.Catalog.Users;
using ProductStoreAPI.Core.Entities;
using ProductStoreAPI.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProductStoreAPI.Infrastructure.Implement.Catalog.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly ProductStoreDbContext _context;
        private readonly IMapper _mapper;
        private readonly IBCryptRepository _bCryptRepository;
        private readonly IConfiguration _configuration;

        public UserRepository(ProductStoreDbContext context, IMapper mapper, IBCryptRepository bCryptRepository, IConfiguration configuration)
        {
            _context = context;
            _mapper = mapper;
            _bCryptRepository = bCryptRepository;
            _configuration = configuration;
        }

        public class TokenModal
        {
            public string AccessToken { get; set; } = string.Empty;
            public string RefreshToken { get; set; } = string.Empty;
        }

        // Hàm thu hồi tất cả refresh token cũ
        private async Task RevokeOldRefreshTokenAsync(int userId)
        {
            var refreshTokens = await _context.RefreshTokens.Where(x => x.UserId == userId && !x.IsRevoked).ToListAsync();

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.IsRevoked = true;
            }

            await _context.SaveChangesAsync();
        }

        private async Task<TokenModal> GenerateToken(UserResponseDto userResponseDto)
        {
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId", userResponseDto.Id.ToString()),
                    new Claim("Email", userResponseDto.Email),
                    new Claim("RoleId", userResponseDto.RoleId.ToString()),
                    new Claim("RoleName", userResponseDto.RoleName),
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var accessTokenBase = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: signIn
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(accessTokenBase);
            var refreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
            var refreshTokenRequest = new RefreshTokenRequestDto
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                UserId = userResponseDto.Id,
            };
            var refreshTokenUpload = _mapper.Map<RefreshToken>(refreshTokenRequest);

            // Thu hồi tất cả refreshToken cũ, trước khi lưu mới vào database
            await RevokeOldRefreshTokenAsync(userResponseDto.Id);

            // Lưu refreshToken vào database
            _context.RefreshTokens.Add(refreshTokenUpload);
            await _context.SaveChangesAsync();

            return new TokenModal
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        public async Task<ApiResult<UserResponseSignInDto>> SignInAsync(UserRequestSignInDto userRequestSignInDto)
        {
            try
            {
                var existingUser = _context.Users.Include(x => x.Role)
                    .FirstOrDefault(x => x.Email == userRequestSignInDto.Email);

                if (existingUser == null)
                {
                    return new ApiErrorResult<UserResponseSignInDto>("Email không tồn tại!");
                }

                if (!_bCryptRepository.VerifyPassword(existingUser.PasswordHash, userRequestSignInDto.Password))
                {
                    return new ApiErrorResult<UserResponseSignInDto>("Mật khẩu không đúng!");
                }

                var userResponse = _mapper.Map<UserResponseDto>(existingUser);
                var tokenModal = await GenerateToken(userResponse);

                var dataResponse = new UserResponseSignInDto
                {
                    User = userResponse,
                    AccessToken = tokenModal.AccessToken,
                    RefreshToken = tokenModal.RefreshToken,
                };

                return new ApiSuccessResult<UserResponseSignInDto>(dataResponse, "Đăng nhập thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<UserResponseSignInDto>("Lỗi khi đăng nhập tài khoàn: " + ex.Message);
            }
        }

        public async Task<ApiResult<string>> SignUpAsync(UserRequestSignUpDto userRequestSignUpDto)
        {
            try
            {
                var existingUser = _context.Users.FirstOrDefault(x => x.Email == userRequestSignUpDto.Email);

                if (existingUser != null)
                {
                    return new ApiErrorResult<string>("Địa chỉ email này đã tồn tại!");
                }

                if (userRequestSignUpDto.Password != userRequestSignUpDto.ConfirmPassword)
                {
                    return new ApiErrorResult<string>("Bắt buộc mật khẩu và xác nhận mật khẩu phải giống nhau!");
                }

                var user = _mapper.Map<User>(userRequestSignUpDto);
                user.PasswordHash = _bCryptRepository.HashPassword(userRequestSignUpDto.Password);
                user.RoleId = 2;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return new ApiSuccessResult<string>("Đăng ký tài khoản thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<string>("Lỗi khi đăng ký tài khoản: " + ex.Message);
            }
        }

        public async Task<ApiResult<UserResponseRefreshTokenDto>> RefreshTokenAsync(UserRequestRefreshTokenDto userRequestRefreshTokenDto)
        {
            try
            {
                var existingRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == userRequestRefreshTokenDto.RefreshToken);

                // Check xem refresh token có trong database không
                if (existingRefreshToken == null)
                {
                    return new ApiErrorResult<UserResponseRefreshTokenDto>("Refresh token không hợp lệ!");
                }

                // Check xem refresh token đã bị thu hồi chưa
                if (existingRefreshToken.IsRevoked)
                {
                    return new ApiErrorResult<UserResponseRefreshTokenDto>("Refresh token đã bị thu hồi!");
                }

                // Check xem refresh token còn hạn không
                if (existingRefreshToken.ExpiryDate < DateTime.UtcNow)
                {
                    return new ApiErrorResult<UserResponseRefreshTokenDto>("Refresh token đã hết hạn sử dụng!");
                }

                var existingUser = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Id == existingRefreshToken.UserId);
                var userResponse = _mapper.Map<UserResponseDto>(existingUser);

                var tokenModal = await GenerateToken(userResponse);

                var result = new UserResponseRefreshTokenDto
                {
                    AccessToken = tokenModal.AccessToken,
                    RefreshToken = tokenModal.RefreshToken
                };

                return new ApiSuccessResult<UserResponseRefreshTokenDto>(result, "Refresh token thành công!");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<UserResponseRefreshTokenDto>("Lỗi khi refreshToken: " + ex.Message);
            }
        }
    }
}
