using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.Interfaces.Initializers
{
	public interface IDbInitializer
	{
		Task InitializeDbAsync();
	}
}
