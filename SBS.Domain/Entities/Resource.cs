using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
	
		public class Resource : BaseAuditableEntity<Guid>
		{
			public string Name { get; set; } = string.Empty;
			public int TypeId { get; set; }
			public ResourceType? Type { get; set; }
			public bool IsActive { get; set; } = true;
			public int Capacity { get; set; }
			public TimeSpan OpenAt { get; set; }
			public TimeSpan CloseAt { get; set; }

			public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();
	}
}
