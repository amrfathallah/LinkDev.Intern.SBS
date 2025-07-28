using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SBS.Application.DTOs.Auth
{
    public class TokenDTO
    {
        public String? AccessToken { get; set; }            //Authenticate requests
        public String? RefreshToken { get; set; }        //Get new Access Token
        //public DateTime Expire { get; set; }            //The time when the token will expire
    }
}
