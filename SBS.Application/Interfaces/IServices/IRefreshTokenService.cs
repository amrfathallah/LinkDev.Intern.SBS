using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.Auth;
using SBS.Application.DTOs.Common;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IServices
{
    public interface IRefreshTokenService
    {
        Task StoreRefreshTokenAsync(Guid UserId, string RefreshToken);     // Save RefreshToken in the Database

        Task<bool> IsRefreshTokenValidAsync(Guid UserId, string RefreshToken); // Check the validity of the RefreshToken like (existence, is expired, is revoked)

        Task UpdateRefreshTokenAsync(Guid UserId, string NewRefreshToken); // Update exp. token with new one

        Task RevokeRefreshTokenAsync(Guid UserId); // Revoke token if we found suspicious token 

        Task<ApiResponse<AuthResponseDto>> RefreshExpiredToken(TokenDTO token);
    }
}
