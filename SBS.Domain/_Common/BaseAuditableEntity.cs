using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain._Common
{
	public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey> where TKey : IEquatable<TKey>
	{
		public string CreatedBy { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public string? LastModifiedBy { get; set; }
		public DateTime? LastModifiedAt { get; set; }
	}
}
