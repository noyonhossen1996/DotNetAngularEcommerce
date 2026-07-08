using ECommerceManagement.Application.Common;
using ECommerceManagement.Application.DTOs.Auth;
using ECommerceManagement.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _authService.RegisterAsync(dto);

            return Ok(ApiResponse<AuthResponseDto>
                .Ok(result, "Registration successful."));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _authService.LoginAsync(dto);

            return Ok(ApiResponse<AuthResponseDto>
                .Ok(result, "Login successful."));
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
        {
            var result = await _authService.RefreshTokenAsync(dto);

            return Ok(ApiResponse<AuthResponseDto>
                .Ok(result, "Token refreshed successfully."));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
        {
            await _authService.LogoutAsync(dto);

            return Ok(ApiResponse<string>
                .Ok("Logout successful."));
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? User.FindFirst("sub")?.Value;

            var email = User.FindFirst(ClaimTypes.Email)?.Value
                ?? User.FindFirst("email")?.Value;

            var fullName = User.FindFirst(ClaimTypes.Name)?.Value;

            var role = User.FindFirst(ClaimTypes.Role)?.Value;

            var result = new
            {
                UserId = userId,
                Email = email,
                FullName = fullName,
                Role = role
            };

            return Ok(ApiResponse<object>.Ok(result));
        }

    }
}
