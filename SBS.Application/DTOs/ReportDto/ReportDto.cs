using SBS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.ReportDto
{
    public class ReportDto
    {
        public string Name { get; set; } = string.Empty;
        public ReportTypeEnum reportType { get; set; }
        public Dictionary<string, object> Data { get; set; } = new Dictionary<string, object>();
    }
}
