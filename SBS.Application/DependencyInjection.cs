using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{


			services.AddScoped(typeof(IAuthService), typeof(AuthService));


			return services;
		}
	}
}
