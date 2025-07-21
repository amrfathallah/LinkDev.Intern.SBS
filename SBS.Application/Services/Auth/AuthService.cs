using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SBS.Application.DTOs.Auth;
using SBS.Application.Exceptions;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using System.Security.Claims;

namespace SBS.Application.Services.Auth
{
	public class AuthService(
		UserManager<ApplicationUser> _userManager,
		SignInManager<ApplicationUser> _signInManager,
		ITokenService _tokenService,
		IRefreshTokenService _refreshTokenService) : IAuthService

	{
		public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
		{
			var user = await _userManager.FindByEmailAsync(request.Email)
		   ?? throw new UnAuthorizedException("Invalid email or password");

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

			if (result.IsNotAllowed) throw new UnAuthorizedException("Account not confirmed yet.");

			if (result.IsLockedOut) throw new UnAuthorizedException("Account is Locked.");

			if (!result.Succeeded) throw new UnAuthorizedException("Invalid Login");

			var roles = await _userManager.GetRolesAsync(user);
			var role = roles.FirstOrDefault() ?? throw new UnAuthorizedException("User has no role");


            //TODO: pass roles to GenerateToken method "DONE"
            var token = await _tokenService.GenerateToken(user, role);
			await _refreshTokenService.UpdateRefreshTokenAsync(user.Id, token.RefreshToken, token.Expire);

            var response=  new AuthResponseDto()
			{
				Token = token.AccessToken,
				RefreshToken = token.RefreshToken,
            };

			return response;

		}

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            bool registrationSuccess = existingUser == null;

            if (!registrationSuccess)
                throw new BadRequestException("This email is already in use.");

            var user = new ApplicationUser()
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            registrationSuccess = result.Succeeded;

            if (!registrationSuccess)
                throw new ValidationException() { Errors = result.Errors.Select(E => E.Description) };

            await _userManager.AddToRoleAsync(user, "Employee");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? throw new BadRequestException("User has no role");

            var response = new ApiResponse<AuthResponseDto>
            {
                Success = registrationSuccess,
                Message = registrationSuccess ? "Registration successful. Please log in." : "Registration failed.",
                Data = new AuthResponseDto()
                {
                    Token = null!,
                    RefreshToken = null!
                }
            };
            return response;
        }

		public async Task LogoutAsync(HttpResponse response)
		{
            //TODO: Revoke the refresh token "DONE"
			var userId = response.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _refreshTokenService.RevokeRefreshTokenAsync(Guid.Parse(userId!.ToString()));

			await _signInManager.SignOutAsync();
        }
    }
}
