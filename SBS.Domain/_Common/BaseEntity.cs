using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain._Common
{
	public abstract class BaseEntity<TKey>  where TKey : IEquatable<TKey>
	{
		public TKey Id { get; set; } = default!;
		public bool IsDeleted { get; set; } = false;
	}
}
