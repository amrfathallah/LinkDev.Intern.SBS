using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Domain._Common;
using SBS.Infrastructure.Persistence._Data.Configs._Base;
using System.Reflection.Emit;

namespace SBS.Infrastructure.Persistence._Data.Configs
{
	public class BookingConfiguration : BaseAuditableEntityConfiguration<Booking, Guid>
	{

		public override void Configure(EntityTypeBuilder<Booking> builder)
		{
			builder.HasOne(b => b.User)
				.WithMany(u => u.Bookings)
				.HasForeignKey(b => b.UserId);

			builder.HasOne(b => b.Status)
				.WithMany()
				.HasForeignKey(b => b.StatusId);

			builder.HasOne(b => b.Resource)
				.WithMany(r => r.Bookings)
				.HasForeignKey(b => b.ResourceId);

			builder.Property(b => b.Date).HasColumnType("date");
		}
	}
}
