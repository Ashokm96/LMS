using LMS.Api.Controllers;
using LMS.Api.Models;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.API.Test
{
    [ExcludeFromCodeCoverage]

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

        [Test]
        public void Post_NullUsers_ReturnsErrorMessage()
        {
            // Arrange
            Users nullUsers = null;

            // Act
            var actionResult = _controller.Post(nullUsers);
            var result = actionResult.Value as Result<string>;


            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("User details cannot be null.", result.ErrorMessage);
        }

        [Test]
        public void Post_InvalidEmailFormat_ReturnsBadRequest()
        {
            // Arrange
            Users invalidEmailUsers = new Users { Email = "invalidemail", Password = "validpassword", UserName = "validusername" };

            // Act
            var actionResult = _controller.Post(invalidEmailUsers);
            var result = actionResult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("Invalid email format."));
        }

        [Test]
        public void Post_InvalidPassword_ReturnsBadRequest()
        {
            // Arrange
            Users invalidPasswordUsers = new Users { Email = "validemail@example.com", Password = "short", UserName = "validusername" };

            // Act
            var actionResult = _controller.Post(invalidPasswordUsers);
            var result = actionResult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("Password should be alphanumeric and at least 8 characters."));

        }

        [Test]
        public void Post_UserDetailsAlreadyExist_ReturnsBadRequest()
        {
            // Arrange
            Users existingUser = new Users { Email = "validemail@example.com", Password = "validpassword", UserName = "existinguser" };
            _userServiceMock.Setup(x => x.ValidateUser(existingUser.Email, existingUser.UserName)).ReturnsAsync("invalidUser");

            // Act
            var actionResult = _controller.Post(existingUser);
            var result = actionResult.Result as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("User details already exists."));
        }
    }
}
