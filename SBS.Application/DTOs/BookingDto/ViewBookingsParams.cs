using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.BookingDto
{
	public class ViewBookingsParams
	{

		public int PageIndex { get; set; } = 1;
		public int PageSize { get; set; } = 10;
		public Guid? UserId { get; set; }
		public int? ResourceTypeId { get; set; }
		public int? BookingStatusId { get; set; }
		public DateOnly? From { get; set; }
		public DateOnly? To { get; set; }
		public string? SortBy { get; set; }
		public bool IsDescending { get; set; } = false;



	}
}
