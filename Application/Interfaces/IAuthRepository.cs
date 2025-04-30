using Domain.Models.ResultTypes;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<OperationResult<User>> AuthenticateUser(string email, string password);
        Task<OperationResult<User>> GetUserByEmailAsync(string email);
        OperationResult<string> GenerateJwtToken(User user);
    }
}
