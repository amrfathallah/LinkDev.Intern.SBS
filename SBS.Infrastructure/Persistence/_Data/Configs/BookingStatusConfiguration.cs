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
		}
	}
}
