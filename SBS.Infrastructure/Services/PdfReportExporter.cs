using iTextSharp.text;
using iTextSharp.text.pdf;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SBS.Application.DTOs.ReportDto;
using SBS.Application.Interfaces.IServices;
using SBS.Application.Queries;
using SBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SBS.Infrastructure.Services
{
    public class PdfReportExporter : IPdfReportExporter
    {
        private readonly IMediator _mediator;
        
        public PdfReportExporter(IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task<FileContentResult> Export(ReportTypeEnum reportType, DateOnly? from, DateOnly? to)
        {
            var report = _mediator.Send(new GetReportQuery(reportType, from, to));
            byte[] pdf = GeneratePdf(report.Result);
            var fileName = $"{report.Result.Name}_{DateTime.Now:yyyyMMddHHmmss}.pdf";
            return new FileContentResult(pdf, "application/pdf")
            {
                FileDownloadName = fileName
            };
        }

        public byte[] GeneratePdf(ReportDto report)
        {
            using var stream = new MemoryStream();
            var doc = new iTextSharp.text.Document();
            PdfWriter.GetInstance(doc, stream);
            
            doc.Open();

            var titleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
            var contentFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            doc.Add(new Paragraph(report.Name, titleFont));
            doc.Add(new Paragraph($"Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}", contentFont));
            doc.Add(new Paragraph($"Type: {report.reportType}", contentFont));
            doc.Add(new Paragraph("\n"));

            foreach (var entry in report.Data)
            {
                doc.Add(new Paragraph($"{entry.Key}: {entry.Value}", contentFont));
            }

            doc.Close();

            return stream.ToArray();
        }
    }
}
