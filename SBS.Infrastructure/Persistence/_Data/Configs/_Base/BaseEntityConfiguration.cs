using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Infrastructure.Persistence._Data.Configs._Base
{
	public class BaseEntityConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
	where TEntity : BaseEntity<TKey>
	where TKey : IEquatable<TKey>
	{
		public virtual void Configure(EntityTypeBuilder<TEntity> builder)
		{

			builder.HasKey(e => e.Id);

			builder.Property(E => E.Id)
				.ValueGeneratedOnAdd();
		}
	}
}
