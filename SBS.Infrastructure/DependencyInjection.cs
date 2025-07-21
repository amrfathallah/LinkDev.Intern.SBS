using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces.Initializers;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Services.Auth;
using SBS.Infrastructure.Persistence._Data;
using SBS.Infrastructure.Persistence._Data.Interceptors;
using SBS.Infrastructure.Persistence.Initializers;
using SBS.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			// SBS DbContext
			services.AddDbContext<AppDbContext>((serviceProv, optionsBuilder) =>
			{
				optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
				.AddInterceptors(serviceProv.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
			});




			// Register the IDbInitializer implementation
			services.AddScoped<IDbInitializer, DbInitializer>();



			#region Register Token and Refresh Token

			// Adding Token Service to handle JWT-Token generation
			services.AddScoped<ITokenService, TokenService>();

			// Add RefreshTokenService to handle DB storage and validation
			services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddScoped<IAuthService, AuthService>();
            #endregion

            //services.AddScoped<IDbInitializer, DbInitializer>(); 


            // Register the AuditableEntitySaveChangesInterceptor
            services.AddScoped(typeof(AuditableEntitySaveChangesInterceptor));

			

            return services;
		}
	}
}
