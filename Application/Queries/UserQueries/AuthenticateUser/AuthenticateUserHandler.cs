using Application.DTOs;
using Application.Interfaces;
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
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, OperationResult<AuthenticationResult>>
    {
        private readonly IAuthRepository _authRepository;
        public AuthenticateUserHandler(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }
        public async Task<OperationResult<AuthenticationResult>> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var userToLogin = await _authRepository.GetUserByEmailAsync(request.Email);

            if (userToLogin == null || !BCrypt.Net.BCrypt.Verify(request.Password, userToLogin.Data!.Password))
                return OperationResult<AuthenticationResult>.Failure("Invalid credentials");

            var token = _authRepository.GenerateJwtToken(userToLogin.Data);

            var authResult = new AuthenticationResult(userToLogin.Data, token.Data);

            return OperationResult<AuthenticationResult>.Success(authResult);
        }
    }
}
