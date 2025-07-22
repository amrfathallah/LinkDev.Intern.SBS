using System.Collections.Generic;

namespace SBS.Application.DTOs.ResourceDto
{
    public class ResourceBookedSlotsDto
    {
        public Guid ResourceId { get; set; }
        public string ResourceName { get; set; }
        public List<BookedSlotDto> BookedSlots { get; set; }
    }
} 