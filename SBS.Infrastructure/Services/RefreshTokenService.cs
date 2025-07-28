using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SBS.Application.DTOs.Auth;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Settings;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using SBS.Application.Interfaces.IRepositories;

using System.Security.Claims;


namespace SBS.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService, IActionResult //TODO: remove IActionResult
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public RefreshTokenService(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
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
            return _refreshTokenRepository.StoreRefreshTokenAsync(UserId, RefreshToken);
        }

        public async Task UpdateRefreshTokenAsync(Guid UserId, string NewRefreshToken)
        {
            await _refreshTokenRepository.UpdateRefreshTokenAsync(UserId, NewRefreshToken);
        }

        public async Task<ApiResponse<AuthResponseDto>> RefreshExpiredToken(TokenDTO token)
        {
           return await _refreshTokenRepository.RefreshExpiredToken(token);
        }

        //TODO: remove this
        public Task ExecuteResultAsync(ActionContext context)
        {
            throw new NotImplementedException();
        }
    }
}
