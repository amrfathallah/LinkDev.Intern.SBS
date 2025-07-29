using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain._Common
{
	public interface IBaseAuditableEntity
	{
		string CreatedBy { get; set; }
		DateTime CreatedAt { get; set; }
		string? LastModifiedBy { get; set; }
		DateTime? LastModifiedAt { get; set; }
	}
}
