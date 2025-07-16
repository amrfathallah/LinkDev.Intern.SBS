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
		}
	}
}
