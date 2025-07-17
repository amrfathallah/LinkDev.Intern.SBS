using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.Initializers;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence.Initializers
{
	public abstract class DbInitializer(AppDbContext _dbContext) : IDbInitializer
	{
		public async Task InitializeDbAsync()
		{
			var pendingMigrations = await _dbContext.Database.GetPendingMigrationsAsync();

			if (pendingMigrations.Any())
			{
				await _dbContext.Database.MigrateAsync(); // Update the database to the latest migration
			}
		}

		public abstract Task SeedAsync();
	}
}
