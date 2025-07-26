using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SBS.Application.Interfaces.Initializers;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence.Initializers
{
	public class DbInitializer(AppDbContext _context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole<Guid>> _roleManager) : IDbInitializer
	{
		public async Task InitializeDbAsync()
		{
			var pendingMigrations = await _context.Database.GetPendingMigrationsAsync();

			if (pendingMigrations.Any())
			{
				await _context.Database.MigrateAsync(); // Update the database to the latest migration
			}
		}

        public async Task SeedAsync()
        {

            Console.WriteLine("▶️ Start Seeding Roles...");
            var roles = new[] { "Admin", "Employee" };

            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }

            // Seed admin user
            var adminEmail = "admin@smartbooking.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);

            if (adminUser is null)
            {
                adminUser = new ApplicationUser
                {
                    FullName = "Admin",
                    Email = adminEmail,
                    UserName = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

        }
    }
}
