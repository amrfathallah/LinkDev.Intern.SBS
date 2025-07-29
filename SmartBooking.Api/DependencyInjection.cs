using SBS.Application.Interfaces;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Services;
using SBS.Infrastructure;

namespace SmartBooking.Api
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
