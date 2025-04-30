using Application.Commands.UserCommands.RegisterUser;
using Application.DTOs;
using Application.Interfaces;
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
    public class RegisterUserHandlerTests
    {
        private Mock<IUserRepository> _userRepoMock;
        private RegisterUserHandler _handler;

        [SetUp]
        public void Setup()
        {
            _userRepoMock = new Mock<IUserRepository>();
            _handler = new RegisterUserHandler(_userRepoMock.Object);
        }

        [Test]
        public async Task RegisterUser_Success_ReturnsSuccess()
        {
            // Arrange
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            };

            var command = new RegisterUserCommand
            {
                Email = user.Email,
                Password = "password123"
            };

            _userRepoMock.Setup(repo => repo.RegisterUserAsync(It.IsAny<User>()))
                .ReturnsAsync(OperationResult<User>.Success(user));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(user.Email, result.Data.Email);
            _userRepoMock.Verify(repo => repo.RegisterUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Test]
        public async Task RegisterUser_DuplicateEmail_Failure()
        {
            // Arrange
            var existingUser = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123")
            };

            var command = new RegisterUserCommand
            {
                Email = "test@example.com",
                Password = "password123"
            };

            _userRepoMock.Setup(repo => repo.RegisterUserAsync(It.IsAny<User>()))
                .ReturnsAsync(OperationResult<User>.Failure("Email already in use"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual("Email already in use", result.ErrorMessage);
        }
    }
}
