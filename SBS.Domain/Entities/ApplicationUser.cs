using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string FullName { get; set; } = string.Empty;

		public ICollection<Booking> Bookings { get; set; } = new HashSet<Booking>();

	}
}
