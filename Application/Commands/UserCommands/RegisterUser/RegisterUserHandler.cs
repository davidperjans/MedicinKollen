using Application.Interfaces;
using Domain.Models.ResultTypes;
using Domain.Models.User;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, OperationResult<User>>
    {
        private readonly IUserRepository _userRepository;

        public RegisterUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<OperationResult<User>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(request.Email);
            if (existingUser != null)
                return OperationResult<User>.Failure("Email already in use");


            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var userToRegister = new User
            {
                Email = request.Email,
                Password = hashedPassword,
            };

            return await _userRepository.RegisterUserAsync(userToRegister);
        }
    }
}
