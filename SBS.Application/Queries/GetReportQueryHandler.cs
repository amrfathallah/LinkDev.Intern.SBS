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
            return request.ReportRequest.ReportType switch
            {
                ReportTypeEnum.ResourceUsage => await _reportRepository.GetResourceUsageAsync(request.ReportRequest.From, request.ReportRequest.To),
                ReportTypeEnum.UserActivity => await _reportRepository.GetUserActivityAsync(request.ReportRequest.From, request.ReportRequest.To),
                ReportTypeEnum.PeakHours => await _reportRepository.GetPeakHoursAsync(request.ReportRequest.From, request.ReportRequest.To),
                ReportTypeEnum.BookingTrends => await _reportRepository.GetBookingTrendsAsync(request.ReportRequest.From, request.ReportRequest.To),
                ReportTypeEnum.CancellationStats => await _reportRepository.GetCancellationStatsAsync(request.ReportRequest.From, request.ReportRequest.To),
                ReportTypeEnum.UtilizationRates => await _reportRepository.GetUtilizationRatesAsync(request.ReportRequest.From, request.ReportRequest.To),
                _ => throw new ArgumentException("Invalid report type")
            };
        }
    }
}
