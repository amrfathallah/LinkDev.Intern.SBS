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
        public ReportTypeEnum ReportType { get; set; }
        public List<string> Labels { get; set; } = new List<string>();
        public List<double> Values { get; set; } = new List<double>();  
    }
}
