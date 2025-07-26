using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces;
using SBS.Application.Interfaces.Initializers;
using SBS.Application.Interfaces.IRepositories;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Services.Auth;
using SBS.Infrastructure.Persistence._Data;
using SBS.Infrastructure.Persistence._Data.Interceptors;
using SBS.Infrastructure.Persistence.Initializers;
using SBS.Infrastructure.Repositories;
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




            #region Register IDbInitializer
            services.AddScoped<IDbInitializer, DbInitializer>();
            #endregion

            #region Register Repositories
            services.AddScoped<IResourceRepository,ResourceRepository>();
            #endregion



			#region Register Token and Refresh Token

			// Adding Token Service to handle JWT-Token generation
			services.AddScoped<ITokenService, TokenService>();

			// Add RefreshTokenService to handle DB storage and validation
			services.AddScoped<IRefreshTokenService, RefreshTokenService>();

            services.AddScoped<IAuthService, AuthService>();
            #endregion

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IBookingConflictValidator, BookingConflictValidator>();

            //services.AddScoped<IDbInitializer, DbInitializer>(); 


            // Register the AuditableEntitySaveChangesInterceptor
            services.AddScoped(typeof(AuditableEntitySaveChangesInterceptor));

			

            return services;
		}
	}
}
