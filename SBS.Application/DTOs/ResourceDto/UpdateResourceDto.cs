using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.ResourceDto.ResourceDto
{
    public class UpdateResourceDto
    {
        public string Name { get; set; } = default!;
        public int Capacity { get; set; }
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
    }

}
