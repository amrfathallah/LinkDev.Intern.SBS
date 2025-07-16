using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
	public class Booking : BaseAuditableEntity<Guid>
	{
		public  Guid UserId { get; set; } 
		public ApplicationUser? User { get; set; } 

		public Guid ResourceId { get; set; }
		public Resource? Resource { get; set; }

		public int StatusId { get; set; }
		public BookingStatus? Status { get; set; }

		public DateOnly Date { get; set; }

		public ICollection<BookingSlot> BookingSlots { get; set; } = new HashSet<BookingSlot>();
	}
}
