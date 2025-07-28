using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SBS.Application.DTOs.Auth;
using SBS.Application.Interfaces.IRepositories;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Settings;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using SBS.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository, IRepository<UserRefreshToken>
    {
        private readonly AppDbContext _appDbContext;
        private readonly JWTSettings _jWTSettings;
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        public RefreshTokenRepository(
            IOptions<JWTSettings> options,
            AppDbContext appDb,
            ITokenService tokenService,
            UserManager<ApplicationUser> userManager)
        {
            _appDbContext = appDb;
            _jWTSettings = options.Value;
            _tokenService = tokenService;
            _userManager = userManager;
        }

        public async Task<UserRefreshToken?> GetByIdAsync(Guid id)
        {
            return await _appDbContext.UserRefreshTokens.FindAsync(id);
        }

        public async Task AddAsync(UserRefreshToken instance)
        {
            await _appDbContext.UserRefreshTokens.AddAsync(instance);
        }

        public async Task<List<UserRefreshToken>> GetAllAsync()
        {
            return await _appDbContext.UserRefreshTokens.ToListAsync();
        }

        public async Task UpdateAsync(UserRefreshToken instance)
        {
             _appDbContext.UserRefreshTokens.Update(instance);
        }

        public async Task<bool> IsRefreshTokenValidAsync(Guid UserId, string RefreshToken)
        {
            return await _appDbContext.UserRefreshTokens.AnyAsync(t =>
                t.UserId == UserId
                && t.RefreshToken == RefreshToken
                && t.ExpAt > DateTime.UtcNow
                && !t.IsRevoked
            );
        }

        public async Task RevokeRefreshTokenAsync(Guid UserId)
        {
            // First Revoke all old tokens
            var oldToken = _appDbContext.UserRefreshTokens.Where(t => t.UserId == UserId && !t.IsRevoked);
            foreach (var t in oldToken) { t.IsRevoked = true; }


            await _appDbContext.SaveChangesAsync();

        }

        public Task StoreRefreshTokenAsync(Guid UserId, string RefreshToken)
        {
            var token = new UserRefreshToken
            {
                UserId = UserId,
                RefreshToken = RefreshToken,
                ExpAt = DateTime.UtcNow.AddDays(_jWTSettings.RefreshTokenExpiry),
                CreateAt = DateTime.UtcNow
            };

            _appDbContext.UserRefreshTokens.Add(token);
            return _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(Guid UserId, string NewRefreshToken)
        {
            try
            {
                // Find the latest token for the user
                var token = await _appDbContext.UserRefreshTokens
                    .Where(t => t.UserId == UserId && !t.IsRevoked)
                    .OrderByDescending(t => t.CreateAt)
                    .FirstOrDefaultAsync();

                if (token != null)
                {
                    // Update the existing token record
                    token.RefreshToken = NewRefreshToken;
                    token.ExpAt = DateTime.UtcNow.AddDays(_jWTSettings.RefreshTokenExpiry);
                    token.CreateAt = DateTime.UtcNow;
                }
                else
                {
                    // If no token exists, create a new one
                    var newToken = new UserRefreshToken
                    {
                        UserId = UserId,
                        RefreshToken = NewRefreshToken,
                        ExpAt = DateTime.UtcNow.AddDays(_jWTSettings.RefreshTokenExpiry),
                        CreateAt = DateTime.UtcNow
                    };
                    _appDbContext.UserRefreshTokens.Add(newToken);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the refresh token", ex);
            }

            await _appDbContext.SaveChangesAsync();
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

        //TODO: remove this
        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }

    }
}
