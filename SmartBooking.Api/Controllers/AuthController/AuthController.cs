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
        public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Refresh(TokenDTO tokenDto)
        {
            ApiResponse<AuthResponseDto> response = await _refreshTokenService.RefreshExpiredToken(tokenDto);
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
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