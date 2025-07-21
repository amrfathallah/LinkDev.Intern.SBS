using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SBS.Application.DTOs.Auth;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Settings;
using SBS.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SBS.Application.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly JWTSettings _jWTSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        public TokenService(IOptions<JWTSettings> options, UserManager<ApplicationUser> userManager) {
            _jWTSettings = options.Value;
            _userManager = userManager;
        }

        public async Task<TokenDTO> GenerateToken(ApplicationUser user, string r)
        {
            var Claims = new List<Claim> {                          //List the user information inside the token
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Role, r),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: Claims,
                expires: DateTime.UtcNow.AddMinutes(_jWTSettings.ExpiryMinutes),
                signingCredentials: creds
            );

            TokenDTO newToken = new TokenDTO
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = GenerateRefreshToken(),
                Expire = token.ValidTo,
            };

            return newToken;
        }

        public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public ClaimsPrincipal? GetUserInfoFromExpiredToken(string token)
        {
            var newTokenValidationParameters = new TokenValidationParameters    // Set Validation Petameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, newTokenValidationParameters, out SecurityToken securityToken); // Decode the token and Extract "Claims" and "Security Token"

            if (securityToken is not JwtSecurityToken jwtToken || // Checks if the Token is a JWT
                !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) // Check if the token use the same security algorithm "HmacSha256"
            {
                return null;
            }

            return principal; // Passes the user information to the Refresh controller 
        }

    }
}