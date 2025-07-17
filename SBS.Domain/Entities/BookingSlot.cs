using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
	public class BookingSlot : BaseEntity<Guid>
	{
		public Guid BookingId { get; set; }
		public  Booking? Booking { get; set; }

		public int SlotId { get; set; }
		public  Slot? Slot { get; set; }
	}
}
