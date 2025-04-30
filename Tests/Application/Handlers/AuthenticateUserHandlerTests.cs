using Application.Interfaces;
using Application.Queries.UserQueries.AuthenticateUser;
using AutoMapper;
using Domain.Models.ResultTypes;
using Domain.Models.User;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Application.Handlers
{
    [TestFixture]
    public class AuthenticateUserHandlerTests
    {
        private Mock<IAuthRepository> _authRepoMock;
        private AuthenticateUserHandler _handler;
        [SetUp]
        public void Setup()
        {
            _authRepoMock = new Mock<IAuthRepository>();
            _handler = new AuthenticateUserHandler(_authRepoMock.Object);
        }

        [Test]
        public async Task Handle_ValidCredentials_ReturnsSuccessWithToken()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            };

            var query = new AuthenticateUserQuery(user.Email, "password123");

            _authRepoMock.Setup(_authRepoMock => _authRepoMock.GetUserByEmailAsync(user.Email))
                .ReturnsAsync(OperationResult<User>.Success(user));

            _authRepoMock.Setup(_authRepoMock => _authRepoMock.GenerateJwtToken(user))
                .Returns(OperationResult<string>.Success("token"));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.That(result.Data.Token, Is.EqualTo("token"));
            _authRepoMock.Verify(repo => repo.GetUserByEmailAsync(user.Email), Times.Once);
            _authRepoMock.Verify(repo => repo.GenerateJwtToken(user), Times.Once);
        }

        [Test]
        public async Task Handle_InvalidCredentials_ReturnsFailure()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            };

            var query = new AuthenticateUserQuery(user.Email, "wrongpassword");

            _authRepoMock.Setup(_authRepoMock => _authRepoMock.GetUserByEmailAsync(user.Email))
                .ReturnsAsync(OperationResult<User>.Success(user));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.That(result.ErrorMessage, Is.EqualTo("Invalid credentials"));
            _authRepoMock.Verify(repo => repo.GetUserByEmailAsync(It.IsAny<string>()), Times.Once);
            _authRepoMock.Verify(repo => repo.GenerateJwtToken(It.IsAny<User>()), Times.Never);
        }
    }
}
