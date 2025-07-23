using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.Common
{
	public interface ICurrentUserService
	{
		string? UserId { get; }
	}
}
