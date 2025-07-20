using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly AppDbContext appDbContext;

        public RefreshTokenService(AppDbContext appDb) { appDbContext = appDb; }

        public async Task<bool> IsRefreshTokenValidAsync(Guid UserId, string RefreshToken)
        {
            return await appDbContext.UserRefreshTokens.AnyAsync(t =>
                t.UserId == UserId
                && t.RefreshToken == RefreshToken
                && t.ExpAt > DateTime.UtcNow
                && !t.IsRevoked
            );
        }

        public async Task RevokeRefreshTokenAsync(Guid UserId)
        {
            // First Revoke all old tokens
            var oldToken = appDbContext.UserRefreshTokens.Where(t => t.UserId == UserId && !t.IsRevoked);
            foreach (var t in oldToken) { t.IsRevoked = true; }


            await appDbContext.SaveChangesAsync();

        }

        public Task StoreRefreshTokenAsync(Guid UserId, string RefreshToken, DateTime ExpDate)
        {
            var token = new UserRefreshToken
            {
                UserId = UserId,
                RefreshToken = RefreshToken,
                ExpAt = ExpDate,
                CreateAt = DateTime.UtcNow
            };

            appDbContext.UserRefreshTokens.Add(token);
            return appDbContext.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(Guid UserId, string NewRefreshToken, DateTime NewExpDate)
        {
            // First Revoke all old tokens
            RevokeRefreshTokenAsync(UserId);

            await StoreRefreshTokenAsync(UserId, NewRefreshToken, NewExpDate); 
        }
    }
}
