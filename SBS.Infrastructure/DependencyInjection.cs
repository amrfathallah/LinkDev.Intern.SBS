using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SBS.Application.Interfaces;
using SBS.Application.Interfaces.Initializers;
using SBS.Application.Interfaces.IRepositories;
using SBS.Infrastructure.Persistence._Data;
using SBS.Infrastructure.Persistence.Initializers;
using SBS.Infrastructure.Repositories;
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
            #region SBS DbContext
            services.AddDbContext<AppDbContext>(optionsBuilder =>
            {
                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            #endregion



			#region Register IDbInitializer
			// Register the IDbInitializer implementation
			services.AddScoped<IDbInitializer, DbInitializer>();
			#endregion

            #region Register Repositories
            services.AddScoped<IResourceRepository,ResourceRepository>();
            #endregion

			services.AddScoped<IUnitOfWork, UnitOfWork>();
			services.AddScoped<IBookingConflictValidator, BookingConflictValidator>();

			return services;
		}
	}
}
