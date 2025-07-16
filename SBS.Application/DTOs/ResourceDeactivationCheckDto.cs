using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs
{
    public class ResourceDeactivationCheckDto
    {
        public bool HasBookings { get; set; }
        public List<BookingInfo> Bookings { get; set; } = new();

        public class BookingInfo
        {
            public Guid BookingId { get; set; }
            public string UserName { get; set; } = default!;
            public DateOnly Date { get; set; }
            public List<SlotInfo> Slots { get; set; } = new();
        }

        public class SlotInfo
        {
            public TimeSpan StartAt { get; set; }
            public TimeSpan EndAt { get; set; }
        }
    }

}
