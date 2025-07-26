using Microsoft.AspNetCore.Http;
using SBS.Application.Interfaces.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.CurrentUserService
{
	public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
	{
		private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

		public string? UserId =>
			_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
	}
}
