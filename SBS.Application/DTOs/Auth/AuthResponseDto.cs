using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.Auth
{
	public class AuthResponseDto
	{
		public required string Token { get; set; }
		public required string RefreshToken { get; set; }
	}
}
