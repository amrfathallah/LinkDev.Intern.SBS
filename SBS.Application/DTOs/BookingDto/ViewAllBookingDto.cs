using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.BookingDto
{
	public class ViewAllBookingDto
	{
		public Guid Id { get; set; }
		public required string User { get; set; }
		public DateOnly Date { get; set; }
		public required string ResourceName { get; set; }
		public required string ResourceType { get; set; }
		public required string Status { get; set; }
		public TimeSpan StartTime { get; set; }
		public TimeSpan EndTime { get; set; }
	}
}
