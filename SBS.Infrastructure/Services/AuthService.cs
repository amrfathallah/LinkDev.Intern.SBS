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
		public async Task<ApiResponse< AuthResponseDto>> LoginAsync(LoginRequestDto request)
		{
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Email not Found"
                };

			var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.IsNotAllowed)
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Account is not allowed"
                };


            if (result.IsLockedOut) return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Account Locked"

            };

            if (!result.Succeeded) return new ApiResponse<AuthResponseDto>
            {
                Success = false,
                Message = "Invalid email or password."

            };

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role == null)
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "User has no role assigned."
                };



            //TODO: pass roles to GenerateToken method "DONE"
            var token = await _tokenService.GenerateToken(user, role, null);
			await _refreshTokenService.UpdateRefreshTokenAsync(user.Id, token.RefreshToken);

            var response=  new AuthResponseDto()
			{
				Token = token.AccessToken,
				RefreshToken = token.RefreshToken,
            };

            return new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Login successful.",
                Data = response
            };

		}

        public async Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "The Email is already in use"
                };

            var user = new ApplicationUser()
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.UserName,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "Registration failed.",
                };
            //throw new ValidationException() { Errors = result.Errors.Select(E => E.Description) };

            await _userManager.AddToRoleAsync(user, "Employee");

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();
            if (role == null)
                return new ApiResponse<AuthResponseDto>
                {
                    Success = false,
                    Message = "User has no role assigned."
                };

            // If there is no error, send success response
            return new ApiResponse<AuthResponseDto>
            {
                Success = true,
                Message = "Registration successful.",
            };
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
