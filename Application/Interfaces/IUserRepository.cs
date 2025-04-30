using Domain.Models.ResultTypes;
using Domain.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<OperationResult<User>> RegisterUserAsync(User user);
        Task<OperationResult<User>> GetUserByEmailAsync(string email);
        Task<OperationResult<IEnumerable<User>>> GetAllUsersAsync();
    }
}
