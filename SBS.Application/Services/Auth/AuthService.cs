using Microsoft.AspNetCore.Identity;
using SBS.Application.DTOs.Auth;
using SBS.Application.Exceptions;
using SBS.Application.Interfaces.IServices;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Services.Auth
{
	public class AuthService(
		UserManager<ApplicationUser> _userManager,
		SignInManager<ApplicationUser> _signInManager) : IAuthService
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

			var response=  new AuthResponseDto()
			{
				UserId = user.Id,
				FullName = user.FullName,
				Email = user.Email!,
				Role = role,
				Token = "" //await GenerateTokenAsync()
			};

			return response;

		}

		public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
		{
			var existingUser = await _userManager.FindByEmailAsync(request.Email);
			if (existingUser != null)
				throw new BadRequestException("This email is already in use.");

			var user = new ApplicationUser()
			{
				FullName = request.FullName,
				Email = request.Email,
				UserName = request.UserName,
				EmailConfirmed = true, // Assuming email confirmation is not required for registration
			};

			var result = await _userManager.CreateAsync(user, request.Password);

			if (!result.Succeeded) throw new ValidationException() { Errors = result.Errors.Select(E => E.Description) };

			await _userManager.AddToRoleAsync(user, "Employee");

			var roles = await _userManager.GetRolesAsync(user);

			var response = new AuthResponseDto()
			{
				UserId = user.Id,
				FullName = user.FullName,
				Email = user.Email!,
				Role = roles.FirstOrDefault() ?? "Employee", // the registered user will be an employee by default
				Token = ""  // await GenerateTokenAsync()

			};

			return response;
 
		}
		public async Task<AuthResponseDto> GetCurrentUser(ClaimsPrincipal claimsPrincipal)
		{
			var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

			var user = await _userManager.FindByEmailAsync(email!);

			if (user is null) throw new NotFoundException("User", email!);

			var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() ?? throw new UnAuthorizedException("User has no role");

			return new AuthResponseDto()
			{
				UserId = user!.Id,
				Email = user!.Email!,
				FullName = user.FullName,
				Role = role,
				Token = "" //await GenerateAccessToken()

			};
		}

	}
}
