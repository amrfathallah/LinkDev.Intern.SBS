using SBS.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using SBS.Domain.Entities;

namespace SBS.Application.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<TokenDTO> GenerateToken(ApplicationUser user, string role);                // To Generate JWT-Token for a given "AppUser".
        string GenerateRefreshToken();                                     // For Generating a new refresh token.

        /*
         To refresh the token without forcing the user to login again,
         we should take some info. from the previous "Expired Access token".
         User login -> AccessToken / RefreshToken -> access_token expired -> interceptor detection
         */
        ClaimsPrincipal? GetUserInfoFromExpiredToken(string token);        // Get user details from an expired token, Return NULL if the given Token in unveiled.

    }
}
