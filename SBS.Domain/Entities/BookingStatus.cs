using SBS.Domain._Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Domain.Entities
{
	public class BookingStatus : BaseEntity<int>
	{
		public required string Name { get; set; }
	}
}
