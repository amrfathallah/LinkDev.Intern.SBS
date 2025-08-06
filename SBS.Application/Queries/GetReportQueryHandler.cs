using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SBS.Application.DTOs.ReportDto;
using SBS.Application.Interfaces.IRepositories;
using SBS.Domain.Enums;

namespace SBS.Application.Queries
{
    public class GetReportQueryHandler : IRequestHandler<GetReportQuery, ReportDto>
    {
        private readonly IReportRepository _reportRepository;
        public GetReportQueryHandler(IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        public async Task<ReportDto> Handle(GetReportQuery request, CancellationToken cancellationToken)
        {
            return request.ReportType switch
            {
                ReportTypeEnum.ResourceUsage => await _reportRepository.GetResourceUsageAsync(request.From, request.To),
                ReportTypeEnum.UserActivity => await _reportRepository.GetUserActivityAsync(request.From, request.To),
                ReportTypeEnum.PeakHours => await _reportRepository.GetPeakHoursAsync(request.From, request.To),
                ReportTypeEnum.BookingTrends => await _reportRepository.GetBookingTrendsAsync(request.From, request.To),
                ReportTypeEnum.CancellationStats => await _reportRepository.GetCancellationStatsAsync(request.From, request.To),
                ReportTypeEnum.UtilizationRates => await _reportRepository.GetUtilizationRatesAsync(request.From, request.To),
                _ => throw new ArgumentException("Invalid report type")
            };
        }
    }
}
