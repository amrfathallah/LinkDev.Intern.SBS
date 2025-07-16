using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SBS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Infrastructure.Persistence._Data.Configs._Base;

namespace SBS.Infrastructure.Persistence._Data.Configs
{
	public class BookingStatusConfiguration : BaseEntityConfiguration<BookingStatus, int>
	{
		public override void Configure(EntityTypeBuilder<BookingStatus> builder)
		{
			builder.Property(bs => bs.Name)
				.IsRequired()
				.HasMaxLength(50);

			builder.HasData(
					new BookingStatus { Id = 1, Name = "Upcoming" },
					new BookingStatus { Id = 2, Name = "Happening" },
					new BookingStatus { Id = 3, Name = "Finished" });
		}
	}
}
