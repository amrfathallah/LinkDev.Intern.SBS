using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SBS.Domain.Entities;
using SBS.Infrastructure.Persistence._Data.Configs._Base;

namespace SBS.Infrastructure.Persistence._Data.Configs
{
	public class ResourceTypeConfiguration : BaseEntityConfiguration<ResourceType, int>
	{
		public override void Configure(EntityTypeBuilder<ResourceType> builder)
		{
			builder.Property(rt => rt.Name)
				.IsRequired()
				.HasMaxLength(50);

			builder.HasData(
		   new ResourceType { Id = 1, Name = "MeetingRoom" },
		   new ResourceType { Id = 2, Name = "Desk" });
		}
	}
}
