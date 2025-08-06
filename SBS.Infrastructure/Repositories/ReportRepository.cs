using Microsoft.EntityFrameworkCore;
using SBS.Application.DTOs.ReportDto;
using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Enums;
using SBS.Infrastructure.Persistence._Data;

namespace SBS.Infrastructure.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _appDbContext;
        public ReportRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        /*
         General Step:
            Filter non-deleted bookings within the given from-to.
         */

        public async Task<ReportDto> GetBookingTrendsAsync(DateOnly? from, DateOnly? to)
        {
            var query = _appDbContext.Bookings
                .Where(b => !b.IsDeleted &&
                    (from.HasValue && b.Date >= from.Value) &&
                    (to.HasValue && b.Date <= to.Value));

            // Groups the bookings per day
            var data = await query
                .GroupBy(b => b.Date)
                .Select(g => new {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Date)
                .ToDictionaryAsync(g => g.Date.ToString("yyyy-MM-dd"), g => (object)g.Count);

            return new ReportDto
            {
                Name = "Booking Trends",
                reportType = ReportTypeEnum.BookingTrends,
                Data = data
            };
        }

        public async Task<ReportDto> GetCancellationStatsAsync(DateOnly? from, DateOnly? to)
        {
            var query = _appDbContext.Bookings
                .Where(b => b.IsDeleted &&
                    (from.HasValue && b.Date >= from.Value) &&
                    (to.HasValue && b.Date <= to.Value));

            // Groups the cancellations per day
            var data = await query
                .GroupBy(b => b.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Date)
                .ToDictionaryAsync(g => g.Date.ToString("yyyy-MM-dd"), g => (object)g.Count);

            return new ReportDto
            {
                Name = "Cancellation Stats",
                reportType = ReportTypeEnum.CancellationStats,
                Data = data
            };
        }

        public async Task<ReportDto> GetPeakHoursAsync(DateOnly? from, DateOnly? to)
        {
            // BookingSlots & Bookings must not be deleted
            // Include Slot and bookings details
            var query = _appDbContext.BookingSlots
                .Include(bs => bs.Booking)
                .Include(bs => bs.Slot)
                .Where(bs => !bs.IsDeleted && !bs.Booking!.IsDeleted &&
                    (from.HasValue && bs.Booking.Date >= from.Value) &&
                    (to.HasValue && bs.Booking.Date <= to.Value));

            // Groups the booking slots by hour of the day
            var data = await query
                .GroupBy(bs => bs.Slot!.StartTime.Hours)
                .Select(g => new
                {
                    Hour = g.Key,
                    Count = g.Count()
                })
                .OrderBy(g => g.Hour)
                .ToDictionaryAsync(g => $"{g.Hour}:00", g => (object)g.Count);

            return new ReportDto
            {
                Name = "Peak Booking Hours",
                reportType = ReportTypeEnum.PeakHours,
                Data = data

            };
        }

        public async Task<ReportDto> GetResourceUsageAsync(DateOnly? from, DateOnly? to)
        {
            // BookingSlots & Bookings must not be deleted
            // Load all related Slots and Resources for each booking
            var query = _appDbContext.Bookings
                .Include(b => b.BookingSlots)
                    .ThenInclude(bs => bs.Slot)
                .Include(b => b.Resource)
                .Where(b => !b.IsDeleted &&
                    (from.HasValue && b.Date >= from.Value) &&
                    (to.HasValue && b.Date <= to.Value));

            // Map ResourceName to the count of booking slots, then count booked slots per resource "in Hour ??"
            var data = await query
                .GroupBy(b => b.Resource!.Name)
                .Select(g => new {
                    Resource = g.Key,
                    Count = g.SelectMany(b => b.BookingSlots).Count()
                })
                .ToDictionaryAsync(g => g.Resource, g => (object)g.Count);

            return new ReportDto { 
                Name = "Resource Usage",
                reportType = ReportTypeEnum.ResourceUsage,
                Data = data
            };

        }

        public async Task<ReportDto> GetUserActivityAsync(DateOnly? from, DateOnly? to)
        {
            var query = _appDbContext.Bookings
                .Include(b => b.User)
                .Include(b => b.BookingSlots)
                    .ThenInclude(bs => bs.Slot)
                .Where(b => !b.IsDeleted &&
                    (from.HasValue && b.Date >= from.Value) &&
                    (to.HasValue && b.Date <= to.Value));

            var data = await query
                .GroupBy(b => b.User!.UserName)
                .Select(g => new {
                    UserName = g.Key,
                    Count = g.SelectMany(b => b.BookingSlots).Count()
                })
                .ToDictionaryAsync(g => g.UserName!.ToString() , g => (object)g.Count);

            return new ReportDto
            {
                Name = "User Activity",
                reportType = ReportTypeEnum.UserActivity,
                Data = data
            };
        }

        public async Task<ReportDto> GetUtilizationRatesAsync(DateOnly? from, DateOnly? to)
        {
            /*
             Goal: Calculate the utilization rates of a specific resource over a specified period.
                Utilization = (Total Booked Time of the resource / Total Available Time of the Resource) * 100
            Assuming that if (from , to) is not provided, we consider 30 days.
             */
            // Step 1: Set the date range
            var fromDate = from ?? DateOnly.FromDateTime(DateTime.Now.AddDays(-30));
            var toDate = to ?? DateOnly.FromDateTime(DateTime.Now);
            var currentDate = fromDate;
            var totalWorkingDays = 0;
            while (currentDate < toDate)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Friday 
                    && currentDate.DayOfWeek != DayOfWeek.Saturday)
                {
                    totalWorkingDays++;
                }
                currentDate = currentDate.AddDays(1);
            }

           // var totalDays = toDate.DayNumber - fromDate.DayNumber + 1; // Total Number of Days "INCLUSIVE"

            //Step 2: Getting bookingSlots with related Booking and Resource data
            var query = _appDbContext.BookingSlots
                .Include(bs => bs.Booking)
                    .ThenInclude(b => b!.Resource)
                .Include(bs => bs.Slot)
                 .Where(bs => !bs.IsDeleted && !bs.Booking!.IsDeleted &&
                    (bs.Booking.Date >= fromDate) &&
                    (bs.Booking.Date <= toDate));

            var date = await query
                .GroupBy(bs => new
                {
                    ResourceId = bs.Booking!.Resource!.Id,
                    ResourceName = bs.Booking.Resource.Name,
                    OpenAt = bs.Booking.Resource.OpenAt,
                    CloseAt = bs.Booking.Resource.CloseAt,
                    SlotStart = bs.Slot!.StartTime, // To avoid EF translation (Translate TimeSpan.TotalHours into Sql Query) Issue.
                    SlotEnd = bs.Slot!.EndTime
                })
                .Select(g => new
                {
                    Resource = g.Key.ResourceName,
                    workingHoursPerDay = (g.Key.CloseAt - g.Key.OpenAt).TotalHours,
                    SlotDuration = (g.Key.SlotEnd - g.Key.SlotStart).TotalHours // Calculate per-slot durations in the backend.
                }).ToListAsync();

            var CalculatedData = date
                .GroupBy(x => new { x.Resource, x.workingHoursPerDay })
                .Select(g => new
                {
                    Resource = g.Key.Resource,
                    WorkingHoursPerDay = g.Key.workingHoursPerDay,
                    TotalBookedTime = g.Sum(x => x.SlotDuration)
                });


            var result = CalculatedData.ToDictionary(
                g => g.Resource,
                h =>
                {
                    double totalAvailableTime = h.WorkingHoursPerDay * totalWorkingDays;
                    double utilizationRate = totalAvailableTime > 0 ? (h.TotalBookedTime / totalAvailableTime) * 100 : 0;
                    return (object)new
                    {
                        totalWorkingDays,
                        totalAvailableTime = Math.Round(totalAvailableTime, 2),
                        TotalBookedTime = Math.Round(h.TotalBookedTime, 2),
                        UtilizationRate1 = Math.Round(utilizationRate, 2),
                    };
                }
            );

            return new ReportDto
            {
                Name = "Utilization Rates",
                reportType = ReportTypeEnum.UtilizationRates,
                Data = result
            };
        }
    }
}