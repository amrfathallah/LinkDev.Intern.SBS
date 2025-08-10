using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.ReportDto;
using SBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.IServices
{
    public interface IExcelReportExporter
    {
        Task<ExportReportDto> Export(ReportRequestDto reportRequest);
    }
}
