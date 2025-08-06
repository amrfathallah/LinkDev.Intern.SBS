using SBS.Application.DTOs.ReportDto;
using SBS.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Queries
{
    public record GetReportQuery(ReportTypeEnum ReportType, DateOnly? From, DateOnly? To) : IRequest<ReportDto>;
}
