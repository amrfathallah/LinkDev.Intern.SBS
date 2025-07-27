using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.Auth;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using SmartBooking.Api.Controllers._Base;
using System.Security.Claims;

namespace SmartBooking.Api.Controllers.AuthController
{
    [ApiController]
    [Route("api/[controller]")] // Route: /api/auth
    public class AuthController : ApiControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(
            IAuthService authService,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _userManager = userManager;
            _tokenService = tokenService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]  // POST: /api/auth/login
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto model)
        {
            var response = await _authService.LoginAsync(model);
            return Ok(response);
        }

        [HttpPost("register")] // POST: /api/auth/register
        [AllowAnonymous]
        public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto registerRequest)
        {
            var response = await _authService.RegisterAsync(registerRequest);
            return Ok(response);
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh(TokenDTO tokenDto)
        {
            // Step 1: Check the expired Access token
            ApplicationUser? user = null;
            try
            {
                user = await _tokenService.refreshExpiredToken(tokenDto);

            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }

            // Step 2: Validate the refresh token
            var validRefToken = await _refreshTokenService.IsRefreshTokenValidAsync(user.Id, tokenDto.RefreshToken);
            if (!validRefToken)
                return Unauthorized("Invalid Refresh token");


            // Step 3: Issue new AccessToken and RefreshToken
            var userRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var newToken = await _tokenService.GenerateToken(user, userRole!, tokenDto.RefreshToken);


            return Ok(newToken);
        }

        [HttpPost("logout")] // POST: /api/auth/logout
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _authService.LogoutAsync(Response);
            return Ok(new { Message = "Logged out successfully." });
        }

    }
}