using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.BookingDto
{
	public class BookingsUsersDto
	{
		public Guid Id { get; set; }
		public string FullName { get; set; } = string.Empty;
	}
}
