using SBS.Application.DTOs.ReportDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IRepositories
{
    public interface IReportRepository
    {
        Task<ReportDto> GetResourceUsageAsync(DateOnly? from, DateOnly? to);
        Task<ReportDto> GetUserActivityAsync(DateOnly? from, DateOnly? to);
        Task<ReportDto> GetPeakHoursAsync(DateOnly? from, DateOnly? to);
        Task<ReportDto> GetBookingTrendsAsync(DateOnly? from, DateOnly? to);
        Task<ReportDto> GetCancellationStatsAsync(DateOnly? from, DateOnly? to);
        Task<ReportDto> GetUtilizationRatesAsync(DateOnly? from, DateOnly? to);
    }
}
