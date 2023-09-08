using LMS.Api.Models;
using LMS.Api.Repository.Interface;
using LMS.Api.Services.Implementation;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace LMS.API.Test
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class UserServiceTests
    {
        private UserService userService;
        private Mock<IUserRepository> userRepositoryMock;
        private Mock<IConfiguration> configurationMock;

        [SetUp]
        public void Setup()
        {
            userRepositoryMock = new Mock<IUserRepository>();
            configurationMock = new Mock<IConfiguration>();

            userService = new UserService(userRepositoryMock.Object, configurationMock.Object);
        }

        [Test]
        public void Register_ValidUser_ReturnsUserId()
        {
            // Arrange
            var user = new Users { UserID = "123", /* Other properties */ };
            userRepositoryMock.Setup(repo => repo.Register(user)).ReturnsAsync(user.UserID);

            // Act
            var result = userService.Register(user).Result;

            // Assert
            Assert.AreEqual(user.UserID, result);
        }

        [Test]
        public void ValidateUser_ValidUser_ReturnsUserId()
        {
            // Arrange
            var userEmail = "test@example.com";
            var userName = "testuser";
            userRepositoryMock.Setup(repo => repo.ValidateUser(userEmail, userName)).ReturnsAsync("123");

            // Act
            var result = userService.ValidateUser(userEmail, userName).Result;

            // Assert
            Assert.AreEqual("123", result);
        }

        [Test]
        public void Authenticate_ValidUser_ReturnsToken()
        {
            // Arrange
            var user = new Users { UserName = "testuser", Role = "user" };
            userRepositoryMock.Setup(repo => repo.GetUser("testuser", "password")).Returns(user);

            configurationMock.SetupGet(config => config["Jwt:SecretKey"]).Returns("YourSecretKeyYourSecretKeyYourSecretKeyYourSecretKeyYourSecretKey");
            configurationMock.SetupGet(config => config["Jwt:Issuer"]).Returns("YourIssuer");
            configurationMock.SetupGet(config => config["Jwt:ExpirationInMinutes"]).Returns("30");

            // Act
            var token = userService.Authenticate("testuser", "password");

            // Assert
            Assert.IsNotNull(token);
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            Assert.AreEqual("YourIssuer", jwtToken.Issuer);

            //var nameClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Name);
            //Assert.IsNotNull(nameClaim, "Claim with type ClaimTypes.Name not found in the token.");
            //Assert.AreEqual("testuser", nameClaim.Value);

            //Assert.AreEqual("YourIssuer", jwtToken.Audiences[0]);
            //Assert.AreEqual("testuser", jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Name).Value);
            //Assert.AreEqual("user", jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Role).Value);
        }

        // Add more test cases for edge cases, invalid inputs, etc.
    }
}
