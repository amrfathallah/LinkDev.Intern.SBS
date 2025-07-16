using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
	public class ResourceType : BaseEntity<int>
	{
		public required string Name { get; set; }
		public ICollection<Resource> Resources { get; set; } = new HashSet<Resource>();
	}
}
