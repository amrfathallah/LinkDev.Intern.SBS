using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data.Configs._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence._Data.Configs
{
	public class SlotConfiguration : BaseAuditableEntityConfiguration<Slot, int>
	{
		public override void Configure(EntityTypeBuilder<Slot> builder)
		{
			builder.Property(s => s.StartTime).IsRequired();

			builder.Property(s => s.EndTime).IsRequired();

			builder.HasData(
			
			new Slot { Id = 1, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder" },
			new Slot { Id = 2, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder" },
			new Slot {Id = 3, StartTime = new TimeSpan(11, 0, 0), EndTime = new TimeSpan(12, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder" },
			new Slot {Id = 4, StartTime = new TimeSpan(12, 0, 0), EndTime = new TimeSpan(13, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder" },
			new Slot {Id = 5, StartTime = new TimeSpan(13, 0, 0), EndTime = new TimeSpan(14, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder" },
			new Slot {Id = 6, StartTime = new TimeSpan(14, 0, 0), EndTime = new TimeSpan(15, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder"	 },
			new Slot {Id = 7, StartTime = new TimeSpan(15, 0, 0), EndTime = new TimeSpan(16, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder"	 },
			new Slot {Id = 8, StartTime = new TimeSpan(16, 0, 0), EndTime = new TimeSpan(17, 0, 0), IsActive = true, CreatedAt = DateTime.UtcNow, CreatedBy = "Seeder", LastModifiedAt = DateTime.UtcNow, LastModifiedBy = "Seeder" }
		);
		}
	}
}
