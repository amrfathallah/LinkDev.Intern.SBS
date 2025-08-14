using SBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.ReportDto
{
    public class ReportRequestDto
    {
        public ReportTypeEnum? ReportType { get; set; }
        public ExportTypeEnum? ExportType { get; set; }
        public DateOnly? From { get; set; }
        public DateOnly? To { get; set; }
    }
}
