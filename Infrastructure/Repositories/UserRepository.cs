using Application.Interfaces;
using Domain.Models.ResultTypes;
using Domain.Models.User;
using Infrastructure.Database;
using Infrastructure.Repositories.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OperationResult<User>> RegisterUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return await Task.FromResult(OperationResult<User>.Success(user));
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
        public async Task<OperationResult<User>> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                return OperationResult<User>.Failure("User not found");

            return OperationResult<User>.Success(user);
        }
        public async Task<OperationResult<IEnumerable<User>>> GetAllUsersAsync()
        {
            try
            {
                var usersFromDb = _context.Users.ToList();
                return await Task.FromResult(OperationResult<IEnumerable<User>>.Success(usersFromDb));
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
