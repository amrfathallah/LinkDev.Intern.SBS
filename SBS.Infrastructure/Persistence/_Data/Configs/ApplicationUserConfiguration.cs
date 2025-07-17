using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence._Data.Configs
{
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(u => u.FullName)
			.IsRequired()
			.HasMaxLength(100);

			builder.HasMany(u => u.Bookings)
				.WithOne(b => b.User)
				.HasForeignKey(b => b.UserId);
		}
	}
}
