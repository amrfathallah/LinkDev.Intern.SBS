using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SBS.Application.DTOs.Auth;
using SBS.Application.DTOs.Common;
using SBS.Application.Interfaces.IRepositories;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Settings;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System.Security.Claims;


namespace SBS.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RefreshTokenService(
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager,
            AppDbContext appDbContext,
            IOptions<JWTSettings> jwtSettings,
            IHttpContextAccessor httpContextAccessor)
        {
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _userManager = userManager;
            _jwtSettings = jwtSettings.Value;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> IsRefreshTokenValidAsync(Guid UserId, string RefreshToken)
        {
            return await _refreshTokenRepository.IsRefreshTokenValidAsync(UserId, RefreshToken);
        }

        public async Task RevokeRefreshTokenAsync(Guid UserId)
        {
            await _refreshTokenRepository.RevokeRefreshTokenAsync(UserId);

        }

        public Task StoreRefreshTokenAsync(Guid UserId, string RefreshToken)
        {
            return _refreshTokenRepository.StoreRefreshTokenAsync(UserId, RefreshToken, _jwtSettings.RefreshTokenExpiry);
        }

        public async Task UpdateRefreshTokenAsync(Guid UserId, string NewRefreshToken)
        {
            await _refreshTokenRepository.UpdateRefreshTokenAsync(UserId, NewRefreshToken, _jwtSettings.RefreshTokenExpiry);
        }

        public async Task<ApiResponse<AuthResponseDto>> RefreshExpiredToken(TokenDTO token)
        {
            try
            {
                // Step 1: Extract claims from exp. Access token
                var principal = _tokenService.GetUserInfoFromExpiredToken(token.AccessToken);
                if (principal == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        Message = "Invalid Access Token",
                        Success = false,
                    };
                }

                // Step 2: Get userId from the extracted claims
                var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        Message = "User Id not found in the token claims",
                        Success = false,
                    };
                }

                // Step 3: Get user from the database
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        Message = "User not found in the database",
                        Success = false,
                    };
                }

                // Step 4: Validate the refresh token
                var isValidRefreshToken = await IsRefreshTokenValidAsync(user.Id, token.RefreshToken);
                if (!isValidRefreshToken)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        Message = "Invalid Refresh Token",
                        Success = false,
                    };
                }

                // Step 5: Get user role
                var role = principal?.FindFirst(ClaimTypes.Role)?.Value;
                if (role == null)
                {
                    return new ApiResponse<AuthResponseDto>
                    {
                        Message = "User role not found in the token claims",
                        Success = false,
                    };
                }

                // Step 6: Generate new token and return response to the controller
                return new ApiResponse<AuthResponseDto>
                {
                    Success = true,
                    Message = "Token refreshed successfully",
                    Data = new AuthResponseDto
                    {
                        Token = (await _tokenService.GenerateToken(user, role, token.RefreshToken)).AccessToken,
                        RefreshToken = token.RefreshToken
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = $"An error occurred while refreshing the token {ex}",
                };
            }
        }
    }
}
