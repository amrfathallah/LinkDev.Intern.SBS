using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SBS.Domain._Common;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data.Configs._Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence._Data.Configs
{
	public class ResourceConfiguration : BaseAuditableEntityConfiguration<Resource, Guid>
	{
		public override void Configure(EntityTypeBuilder<Resource> builder)
		{

			builder.Property(r => r.Name).IsRequired().HasMaxLength(100);

			builder.HasOne(r => r.Type)
				.WithMany(t => t.Resources)
				.HasForeignKey(r => r.TypeId);

			builder.HasMany(r => r.Bookings)
				.WithOne(b => b.Resource)
				.HasForeignKey(b => b.ResourceId);
		}
	}
}
