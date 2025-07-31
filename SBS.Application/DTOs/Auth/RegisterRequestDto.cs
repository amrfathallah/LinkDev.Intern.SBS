using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.Auth
{
	public class RegisterRequestDto
	{
		[Required]
		public required string FullName { get; set; }

		[Required]
		public required string UserName { get; set; }

		[Required]
		[EmailAddress]
		public required string Email { get; set; }

		[Required]
        [RegularExpression(@"(?=^.{6,20}$)(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#%^&*()_+}{"":;'?/>\.<,])(?!.*\s).*$",
			ErrorMessage = "Password must be 6-20 characters and include 1 uppercase, 1 lowercase, 1 number, and 1 special character.")]

        public required string Password { get; set; }
	}
}
