using MediatR;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.ReportDto;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Queries;
using SBS.Domain.Enums;


namespace SmartBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPdfReportExporter _pdfReportExporter;
        private readonly IExcelReportExporter _excelReportExporter;
        public ReportsController
            (
            IMediator mediator,
            IPdfReportExporter pdfReportExporter,
            IExcelReportExporter excelReportExporter
            )
        {
            _mediator = mediator;
            _pdfReportExporter = pdfReportExporter;
            _excelReportExporter = excelReportExporter;
        }

        [HttpGet("report")]
        public async Task<ActionResult<ReportDto>> GetReport([FromQuery] ReportTypeEnum reportType,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        {
            var report = await _mediator.Send(new GetReportQuery(reportType, from, to));
            return Ok(report);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportReport(
            [FromQuery] ReportTypeEnum reportType,
            [FromQuery] ExportTypeEnum exportType,
            [FromQuery] DateOnly? from,
            [FromQuery] DateOnly? to)
        {
            return exportType switch
            {
                ExportTypeEnum.Pdf => Ok(await _pdfReportExporter.Export(reportType, from, to)),
                ExportTypeEnum.Excel => Ok(await _excelReportExporter.Export(reportType, from, to)),
                _ => BadRequest("Unsupported export type.")
            };
        }
    }
}
