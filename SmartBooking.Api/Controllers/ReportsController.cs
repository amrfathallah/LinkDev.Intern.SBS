using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPost("report")]
        [Authorize]
        public async Task<ActionResult<ReportDto>> GetReport(ReportRequestDto reportRequest)
        {
            if (reportRequest == null)
            {
                return BadRequest("Invalid Report Request");
            }

            var report = await _mediator.Send(new GetReportQuery(reportRequest));
            return Ok(report);
        }

        [HttpPost("export")]
        [Authorize]

        public async Task<IActionResult> ExportReport(ReportRequestDto reportRequest)
        {
            return reportRequest.ExportType switch
            {
                ExportTypeEnum.Pdf => Ok(await _pdfReportExporter.Export(reportRequest)),
                ExportTypeEnum.Excel => Ok(await _excelReportExporter.Export(reportRequest)),
                _ => BadRequest("Unsupported export type.")
            };
        }
    }
}
