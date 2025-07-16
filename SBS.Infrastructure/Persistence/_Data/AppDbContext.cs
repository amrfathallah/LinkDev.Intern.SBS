using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence._Data
{
	public class AppDbContext :IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
	{
		public DbSet<Booking> Bookings { get; set; }
		public DbSet<BookingSlot> BookingSlots { get; set; }
		public DbSet<Resource> Resources { get; set; }

		public DbSet<BookingStatus> BookingStatuses { get; set; }
		public DbSet<Slot> Slots { get; set; }

		public DbSet<ResourceType> ResourceTypes { get; set; }




		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

		
		}

	}
	
}
