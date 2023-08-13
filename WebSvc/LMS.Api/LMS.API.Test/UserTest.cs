using LMS.Api.Controllers;
using LMS.Api.Models;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.API.Test
{
    public class UserTest
    {
        private UserController _controller;
        private Mock<IUserService> _userServiceMock;

        [SetUp]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();
            _controller = new UserController(_userServiceMock.Object);
        }

        [Test]
        public void Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var validLogin = new Login { Username = "validuser", Password = "validpassword" };
            var expectedToken = "validToken";
            _userServiceMock.Setup(x => x.Authenticate(validLogin.Username, validLogin.Password)).Returns(expectedToken);

            // Act
            var actionResult = _controller.Login(validLogin);
            var result = actionResult.Result as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Value);
        }

        [Test]
        public void Login_InvalidCredentials_ReturnsBadRequest()
        {
            // Arrange
            var invalidLogin = new Login { Username = "invaliduser", Password = "invalidpassword" };
            _userServiceMock.Setup(x => x.Authenticate(invalidLogin.Username, invalidLogin.Password)).Returns((string)null);

            // Act
            var actionResult = _controller.Login(invalidLogin);
            var result = actionResult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("unauthorized user"));
        }
    }
}
