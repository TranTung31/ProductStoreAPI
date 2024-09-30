using Microsoft.AspNetCore.Mvc;
using ProductStoreAPI.Application.DTOs.Catalog.Users;
using ProductStoreAPI.Application.Interfaces.Catalog.Users;

namespace ProductStoreAPI.WebAPI.Controllers
{
    [Route("/api/v1/auth")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userService)
        {
            _userRepository = userService;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(UserRequestSignUpDto userRequestSignUpDto)
        {
            var result = await _userRepository.SignUpAsync(userRequestSignUpDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn(UserRequestSignInDto userRequestSignInDto)
        {
            var result = await _userRepository.SignInAsync(userRequestSignInDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(UserRequestRefreshTokenDto userRequestRefreshTokenDto)
        {
            var result = await _userRepository.RefreshTokenAsync(userRequestRefreshTokenDto);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}
