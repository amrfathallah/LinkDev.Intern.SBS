using Microsoft.AspNetCore.Http;
using SBS.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static SBS.Application.Services.Auth.AuthService;

namespace SBS.Application.Interfaces.IServices
{
	public interface IAuthService
	{
        Task<ApiResponse<AuthResponseDto>> RegisterAsync(RegisterRequestDto request);

		Task<ApiResponse<AuthResponseDto>> LoginAsync(LoginRequestDto request);

		Task LogoutAsync(HttpResponse response);

	}
}
