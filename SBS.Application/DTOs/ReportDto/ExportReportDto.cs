using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.ReportDto
{
    public class ExportReportDto
    {
        public string FileName { get; set; } = "";
        public string FileObjectUrl { get; set; } = "";
        public string ContentType { get; set; } = "application/octet-stream";

        public static ExportReportDto FromBinary(byte[] fileContent, string fileName, string contentType = "application/octet-stream")
        {
            return new ExportReportDto
            {
                FileName = fileName,
                FileObjectUrl = $"data:{contentType};base64,{Convert.ToBase64String(fileContent)}",
                ContentType = contentType
            };
        }
    }
}
