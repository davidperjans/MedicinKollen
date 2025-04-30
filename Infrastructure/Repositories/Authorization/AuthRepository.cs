using Application.Interfaces;
using Domain.Models.ResultTypes;
using Domain.Models.User;
using Infrastructure.Database;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Authorization
{
    public class AuthRepository : IAuthRepository
    {
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public AuthRepository(IConfiguration config, AppDbContext context)
        {
            _config = config;
            _context = context;
        }
        public Task<OperationResult<User>> AuthenticateUser(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user == null)
                return Task.FromResult(OperationResult<User>.Failure("Invalid credentials"));

            return Task.FromResult(OperationResult<User>.Success(user));
        }

        public Task<OperationResult<User>> GetUserByEmailAsync(string email)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email);

            if (user == null)
                return Task.FromResult(OperationResult<User>.Failure("User not found"));

            return Task.FromResult(OperationResult<User>.Success(user));
        }

        public OperationResult<string> GenerateJwtToken(User user)
        {
            try
            {
                var key = Encoding.ASCII.GetBytes(_config["JWT:Key"]!);
                if (key.Length == 0)
                {
                    return OperationResult<string>.Failure("JWT secret key is missing in the configuration.");
                }

                int expireTime = int.Parse(_config["JWT:ExpireTime"]!);

                // Skapa säkerhetstokenbeskrivningen
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(expireTime),
                    Issuer = _config["JWT:Issuer"],
                    Audience = _config["JWT:Audience"],
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                // Skapa och skriv ut JWT-token
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);

                return OperationResult<string>.Success(jwtToken);
            }
            catch (Exception ex)
            {
                // Logga eller hantera eventuella undantag
                return OperationResult<string>.Failure($"Token generation failed: {ex.Message}");
            }
        }
    }
}
