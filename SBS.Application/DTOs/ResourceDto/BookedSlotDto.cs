namespace SBS.Application.DTOs.ResourceDto
{
    public class BookedSlotDto
    {
        public int SlotId { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
} 