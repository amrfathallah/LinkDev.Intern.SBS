namespace SBS.Application.DTOs.ResourceDto
{
    public class GetBookedSlotsRequestDto
    {
        public Guid ResourceId { get; set; }
        public DateTime Date { get; set; }
    }
} 