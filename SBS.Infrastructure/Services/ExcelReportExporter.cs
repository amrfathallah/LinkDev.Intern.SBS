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
        public async Task<ExportReportDto> Export(ReportTypeEnum reportType, DateOnly? from, DateOnly? to)
        {
            var report = await _mediator.Send(new GetReportQuery(reportType, from, to));
            var excel = GenerateExcel(report);
            var fileName = $"{reportType}_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
            return ExportReportDto.FromBinary(
                excel,
                fileName,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            );
        }

        public byte[] GenerateExcel(ReportDto report)
        {
            using var sheet = new XLWorkbook();
            var worksheet = sheet.Worksheets.Add(report.Name);

            worksheet.Cell(1, 1).Value = "Report Name";
            worksheet.Cell(1, 2).Value = report.Name;
            worksheet.Cell(2, 1).Value = "Report Type";
            worksheet.Cell(2, 2).Value = report.ReportType.ToString();

            worksheet.Cell(4, 1).Value = "Key";
            worksheet.Cell(4, 2).Value = "Value";

            int row = 5;
            foreach (var label in report.Labels)
            {
                worksheet.Cell(row, 1).Value = label;
                row++;
            }
            row = 5;
            foreach (var entry in report.Values)
            {
                worksheet.Cell(row, 2).Value = entry;
                row++;
            }

            using var stream = new MemoryStream();
            sheet.SaveAs(stream);
            return stream.ToArray();
        }


    }
}
