using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.Auth
{
	public class AuthResponseDto
	{
		public required Guid UserId { get; set; }
		public required string FullName { get; set; }
		public required string Email { get; set; }
		public required string Role { get; set; }
		public required string Token { get; set; }
	}
}
