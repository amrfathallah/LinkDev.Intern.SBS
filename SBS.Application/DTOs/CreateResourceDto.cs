using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs
{
    public class CreateResourceDto
    {
        public string Name { get; set; } = default!;
        public int TypeId { get; set; }
        public int Capacity { get; set; }
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
    }
}
