using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.BookingDto
{
	public record BookingRequestDto(
		[Required] Guid ResourceId,
		[Required] DateOnly Date,
		[Required, MinLength(1)] List<int> SlotsIds
	);
}
