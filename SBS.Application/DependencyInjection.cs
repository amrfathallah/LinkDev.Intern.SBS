using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Interfaces;
using SBS.Application.Services;

namespace SBS.Application
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddScoped<IBookingService, BookingService>();

			


			return services;
		}
	}
}
