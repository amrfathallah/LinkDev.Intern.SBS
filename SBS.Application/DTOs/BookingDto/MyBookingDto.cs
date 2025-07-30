

using SBS.Domain.Enums;

namespace SBS.Application.DTOs.BookingDto
{
	public record MyBookingDto(
        Guid Id,
        Guid ResourceId,
        DateOnly Date,
        BookingStatusEnum Status,
        TimeSpan StartTime,
        TimeSpan EndTime
    );
}
