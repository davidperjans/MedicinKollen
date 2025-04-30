using Application.DTOs;
using Domain.Models.ResultTypes;
using Domain.Models.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Queries.UserQueries.AuthenticateUser
{
    public class AuthenticateUserQuery : IRequest<OperationResult<AuthenticationResult>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public AuthenticateUserQuery(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
