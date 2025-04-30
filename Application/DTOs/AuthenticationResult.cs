using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class AuthenticationResult
    {
        public User User { get; set; }
        public string Token { get; set; }

        public AuthenticationResult(User user, string token)
        {
            User = user;
            Token = token;
        }
    }
}
