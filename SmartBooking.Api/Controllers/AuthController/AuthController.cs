using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.Auth;
using SBS.Application.Interfaces.IServices;
using SmartBooking.Api.Controllers._Base;

namespace SmartBooking.Api.Controllers.AuthController
{
	public class AuthController (IAuthService _authService) : ApiControllerBase
	{
		[HttpPost("login")]  // POST: /api/auth/login
		[AllowAnonymous]

		public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto model)
		{
			var response  = await _authService.LoginAsync(model);

			return Ok(response);
		}

		[HttpPost("register")] // POST: /api/auth/register
		[AllowAnonymous]

		public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto registerRequest)
		{
			var response = await _authService.RegisterAsync(registerRequest);
			return Ok(response);
		}

		[HttpPost("logout")] // POST: /api/auth/logout
		[Authorize]
		public async Task<ActionResult> Logout()
		{
			await _authService.LogoutAsync(Response);
			return Ok(new { Message = "Logged out successfully." });
		}


		[HttpGet("me")] // GET: /api/auth/me
		[Authorize]
		public async Task<ActionResult<AuthResponseDto>> GetCurrentUser()
		{
			var result = await _authService.GetCurrentUser(User);

			return Ok(result);
		}
	}
}
