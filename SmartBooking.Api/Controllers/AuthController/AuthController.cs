using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.Auth;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using System.Security.Claims;

namespace SmartBooking.Api.Controllers.AuthController
{
    [ApiController]

    [Route("api/[controller]")] // as we have the route to : /api/auth
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenDTO tokenDto)
        {
            // Step 1: Extract claims from exp. Access token
            var principal = _tokenService.GetUserInfoFromExpiredToken(tokenDto.RefreshToken);
            if (principal == null)
                return Unauthorized("Invalid Access Token");

            // Step 2: Get userId from the extracted claims
            var userId = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized("User not found in the token");

            // Step 3: Get user from the database
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Unauthorized("User not found in the database");

            // Step 4: Validate the new refresh token
            var validRefToken = await _refreshTokenService.IsRefreshTokenValidAsync(user.Id, tokenDto.RefreshToken);
            if (!validRefToken)
                return Unauthorized("Invalid Refresh token");

            // Step 5: Issue new AccessToken and RefreshToken
            var newToken = await _tokenService.GenerateToken(user);

            // Step 6: Update old RefreshToken with the new one in the database
            await _refreshTokenService.UpdateRefreshTokenAsync(user.Id, newToken.RefreshToken, newToken.Expire);

            return Ok(newToken);
        }
        
    }
}
