using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;

namespace SBS.Infrastructure.Repositories
{
    public class RefreshTokenRepository : GenericRepository<UserRefreshToken>, IRefreshTokenRepository
    {
        private readonly AppDbContext _appDbContext;
        public RefreshTokenRepository(AppDbContext appDb) : base(appDb)
        {
            _appDbContext = appDb;
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
            var oldToken = _appDbContext.UserRefreshTokens.Where(t => t.UserId == UserId && !t.IsRevoked);
            foreach (var t in oldToken) { t.IsRevoked = true; }


            await _appDbContext.SaveChangesAsync();

        }

        public Task StoreRefreshTokenAsync(Guid UserId, string RefreshToken, int refExp)
        {
            var token = new UserRefreshToken
            {
                UserId = UserId,
                RefreshToken = RefreshToken,
                ExpAt = DateTime.UtcNow.AddDays(refExp),
                CreateAt = DateTime.UtcNow
            };

            _appDbContext.UserRefreshTokens.Add(token);
            return _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenAsync(Guid UserId, string NewRefreshToken, int refExp)
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
                    token.ExpAt = DateTime.UtcNow.AddDays(refExp);
                    token.CreateAt = DateTime.UtcNow;
                }
                else
                {
                    // If no token exists, create a new one
                    var newToken = new UserRefreshToken
                    {
                        UserId = UserId,
                        RefreshToken = NewRefreshToken,
                        ExpAt = DateTime.UtcNow.AddDays(refExp),
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

    }
}
