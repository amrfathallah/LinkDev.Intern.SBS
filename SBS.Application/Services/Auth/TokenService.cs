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

        public async Task<TokenDTO> GenerateToken(ApplicationUser user, string role, string?validRefreshToken)
        {
            var claims = new List<Claim> {                          //List the user information inside the token
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email!),
                new(ClaimTypes.Name, user.FullName),
                new(ClaimTypes.Role, role),
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jWTSettings.Issuer,
                audience: _jWTSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jWTSettings.AccessTokenExpiry),
                signingCredentials: creds
            );

             TokenDTO newToken = new TokenDTO
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = validRefreshToken ?? GenerateRefreshToken(),
             };

            return newToken;
        }

        public string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        public ClaimsPrincipal? GetUserInfoFromExpiredToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                return null;

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

        public async Task<ApplicationUser> refreshExpiredToken(TokenDTO token)
        {
            // Step 1: Extract claims from exp. Access token
            var principal = GetUserInfoFromExpiredToken(token.AccessToken);
            if (principal == null)
                throw new UnauthorizedAccessException("Invalid Access Token");

            // Step 2: Get userId from the extracted claims
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                throw new UnauthorizedAccessException("User ID not found in the token claims");

            // Step 3: Get user from the database
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                throw new UnauthorizedAccessException("User not found in the database");

            // Step 4: Check if the token is Expired or no

            return user;
        }
    }
}