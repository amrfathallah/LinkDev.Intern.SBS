using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.ReportDto;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Queries;
using SBS.Domain.Enums;

namespace SBS.Infrastructure.Services
{
    public class ExcelReportExporter : IExcelReportExporter
    {
        private readonly IMediator _mediator;
        
        public ExcelReportExporter(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<FileContentResult> Export(ReportTypeEnum reportType, DateOnly? from, DateOnly? to)
        {
            var report = await _mediator.Send(new GetReportQuery(reportType, from, to));
            byte[] excel = GenerateExcel(report);
            var fileName = $"{reportType}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return new FileContentResult(excel, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
        }

        public byte[] GenerateExcel(ReportDto report)
        {
            using var sheet = new XLWorkbook();
            var worksheet = sheet.Worksheets.Add(report.Name);

            worksheet.Cell(1, 1).Value = "Report Name";
            worksheet.Cell(1, 2).Value = report.Name;
            worksheet.Cell(2, 1).Value = "Report Type";
            worksheet.Cell(2, 2).Value = report.reportType.ToString();

            worksheet.Cell(4, 1).Value = "Key";
            worksheet.Cell(4, 2).Value = "Value";

            int row = 5;
            foreach (var entry in report.Data)
            {
                worksheet.Cell(row, 1).Value = entry.Key;
                worksheet.Cell(row, 2).Value = entry.Value?.ToString();
                row++;
            }

            using var stream = new MemoryStream();
            sheet.SaveAs(stream);
            return stream.ToArray();
        }


    }
}
