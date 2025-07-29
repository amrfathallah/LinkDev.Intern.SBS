using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.BookingDto
{
	public class ViewBookingsFilterDto
	{

		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public Guid? UserId { get; set; }
		public string? ResourceType { get; set; }
		public string? Status { get; set; }
		public DateTime? From { get; set; }
		public DateTime? To { get; set; }
		public string? SortBy { get; set; }
		public string? SortDirection { get; set; } = "asc";

	}
}
