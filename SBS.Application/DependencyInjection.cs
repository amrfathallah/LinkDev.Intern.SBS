using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Mapping;
using SBS.Application.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
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
			services.AddScoped<IResourceService, ResourceService>();
			services.AddAutoMapper(Mapper => Mapper.AddProfile(new MappingProfile()));
			services.AddScoped<IBookingService, BookingService>();

			


            return services;
		}
	}
}
