using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
    public class UserRefreshToken : BaseEntity <Guid>
    {
        //public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime ExpAt { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
