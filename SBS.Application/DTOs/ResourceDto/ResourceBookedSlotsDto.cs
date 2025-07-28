using System.Collections.Generic;

namespace SBS.Application.DTOs.ResourceDto
{
    public class ResourceBookedSlotsDto
    {
        public Guid ResourceId { get; set; }
        public required string ResourceName { get; set; }
        public required List<BookedSlotDto> BookedSlots { get; set; }
    }
} 