using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.BookingDto
{
	public record BookingRequestDto(Guid ResourceId, DateOnly Date, List<int> SlotsIds);
}
