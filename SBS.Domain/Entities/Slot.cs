using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
	public class Slot : BaseAuditableEntity<int>
	{
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
		public bool IsActive { get; set; } = true;

		public ICollection<BookingSlot> BookingSlots { get; set; } = new HashSet<BookingSlot>();
	}
}
