using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.ResourceDto
{
    public class ResourceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public int TypeId { get; set; }
        public string TypeName { get; set; } = default!;
        public int Capacity { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan OpenAt { get; set; }
        public TimeSpan CloseAt { get; set; }
    }

}
