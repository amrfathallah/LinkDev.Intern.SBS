using SBS.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task StoreRefreshTokenAsync(Guid UserId, string RefreshToken, int refExp);     // Save RefreshToken in the Database

        Task<bool> IsRefreshTokenValidAsync(Guid UserId, string RefreshToken); // Check the validity of the RefreshToken like (existence, is expired, is revoked)

        Task UpdateRefreshTokenAsync(Guid UserId, string NewRefreshToken, int refExp); // Update exp. token with new one

        Task RevokeRefreshTokenAsync(Guid UserId); // Revoke token if we found suspicious token 
    }
}
