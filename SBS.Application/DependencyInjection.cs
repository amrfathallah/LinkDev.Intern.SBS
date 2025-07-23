using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Mapping;
using SBS.Application.Services;
using SBS.Application.Services.Auth;
using IResourceService = SBS.Application.Interfaces.IServices.IResourceService;

namespace SBS.Application
{
    public static class DependencyInjection
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
		{
			//services.AddScoped<IBookingService, IBookingService>();
            services.AddScoped<IResourceService, ResourceService>();

			//services.AddAutoMapper(typeof(MappingProfile).Assembly); 
			services.AddAutoMapper(Mapper => Mapper.AddProfile(new MappingProfile()));

			services.AddScoped(typeof(IAuthService), typeof(AuthService));

            return services;
		}
	}
}