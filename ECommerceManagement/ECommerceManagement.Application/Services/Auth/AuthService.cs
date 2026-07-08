using ECommerceManagement.Application.DTOs.Auth;
using ECommerceManagement.Application.Exceptions;
using ECommerceManagement.Application.Interfaces;
using ECommerceManagement.Application.Interfaces.Auth;
using ECommerceManagement.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace ECommerceManagement.Application.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtService _jwtService;
        private readonly PasswordHasher<ApplicationUser> _passwordHasher = new();

        public AuthService(IUnitOfWork unitOfWork, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = (await _unitOfWork.Users.FindAsync(x => x.Email == dto.Email))
                .FirstOrDefault();

            if (existingUser is not null)
                throw new BadRequestException("Email already exists.");

            var user = new ApplicationUser
            {
                FullName = dto.FullName,
                Email = dto.Email,
                Role = "Customer"
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            var refreshToken = new RefreshToken
            {
                User = user,
                Token = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = (await _unitOfWork.Users.FindAsync(x => x.Email == dto.Email))
                .FirstOrDefault();

            if (user is null)
                throw new BadRequestException("Invalid email or password.");

            var result = _passwordHasher.VerifyHashedPassword(
                user,
                user.PasswordHash,
                dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new BadRequestException("Invalid email or password.");

            var refreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            await _unitOfWork.RefreshTokens.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
        {
            var refreshToken = (await _unitOfWork.RefreshTokens
                    .FindAsync(x => x.Token == dto.RefreshToken && !x.IsRevoked))
                .FirstOrDefault();

            if (refreshToken is null || refreshToken.ExpiresAt < DateTime.UtcNow)
                throw new BadRequestException("Invalid refresh token.");

            var user = await _unitOfWork.Users.GetByIdAsync(refreshToken.UserId);

            if (user is null)
                throw new BadRequestException("Invalid refresh token.");

            refreshToken.IsRevoked = true;

            var newRefreshToken = new RefreshToken
            {
                UserId = user.Id,
                Token = _jwtService.GenerateRefreshToken(),
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _unitOfWork.RefreshTokens.Update(refreshToken);
            await _unitOfWork.RefreshTokens.AddAsync(newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Role = user.Role,
                AccessToken = _jwtService.GenerateAccessToken(user),
                RefreshToken = newRefreshToken.Token
            };
        }

        public async Task LogoutAsync(RefreshTokenRequestDto dto)
        {
            var refreshToken = (await _unitOfWork.RefreshTokens
                    .FindAsync(x => x.Token == dto.RefreshToken && !x.IsRevoked))
                .FirstOrDefault();

            if (refreshToken is null)
                throw new BadRequestException("Invalid refresh token.");

            refreshToken.IsRevoked = true;

            _unitOfWork.RefreshTokens.Update(refreshToken);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
