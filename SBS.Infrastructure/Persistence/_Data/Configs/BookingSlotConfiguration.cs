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
	public class BookingSlotConfiguration : BaseEntityConfiguration<BookingSlot, Guid>
	{
		public override void Configure(EntityTypeBuilder<BookingSlot> builder)
		{
			builder.HasOne(bs => bs.Booking)
				.WithMany(b => b.BookingSlots)
				.HasForeignKey(bs => bs.BookingId);

			builder.HasOne(bs => bs.Slot)
				.WithMany(s => s.BookingSlots)
				.HasForeignKey(bs => bs.SlotId);
		}
	}
}
