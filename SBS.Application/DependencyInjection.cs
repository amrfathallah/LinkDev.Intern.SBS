using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Mapping;
using SBS.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SBS.Application
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddAutoMapper(Mapper => Mapper.AddProfile(new MappingProfile()));
			services.AddScoped<IBookingService, BookingService>();
			services.AddScoped<IResourceService, ResourceService>();
			services.AddScoped<ISlotService, SlotService>();

			


            return services;
		}
	}
}
