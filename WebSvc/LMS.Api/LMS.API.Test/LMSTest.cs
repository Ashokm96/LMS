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
    public class LMSTest
    {

        private Mock<ILMSService> lmsServiceMock;
        private Mock<ILogger<LMSController>> mockLogger;
        private LMSController lmsController;

        [SetUp]
        public void Setup()
        {
            lmsServiceMock = new Mock<ILMSService>();
            mockLogger = new Mock<ILogger<LMSController>>();
            lmsController = new LMSController(lmsServiceMock.Object);
        }


        [Test]
        public async Task DeleteByCourseName_ValidCourse_DeletesCourse()
        {
            // Arrange
            string courseName = "SampleCourse";
            lmsServiceMock.Setup(s => s.GetCourse(courseName)).ReturnsAsync(new Course());

            // Act
            var result = await lmsController.DeleteByCouseName(courseName) as OkObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(200));
            Assert.That(result.Value, Is.EqualTo("Course deleted Successfully"));
        }

        [Test]
        public async Task DeleteByCourseName_CourseNotFound_ReturnsBadRequest()
        {
            // Arrange
            string courseName = "NonExistentCourse";
            lmsServiceMock.Setup(s => s.GetCourse(courseName)).ReturnsAsync(null as Course);

            // Act
            var result = await lmsController.DeleteByCouseName(courseName) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("Course details not found!"));
        }

        [Test]
        public async Task DeleteByCourseName_ExceptionOccurs_ReturnsBadRequest()
        {
            // Arrange
            string courseName = "SampleCourse";
            lmsServiceMock.Setup(s => s.GetCourse(courseName)).ThrowsAsync(new Exception());

            // Act
            var result = await lmsController.DeleteByCouseName(courseName) as BadRequestObjectResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.StatusCode, Is.EqualTo(400));
            Assert.That(result.Value, Is.EqualTo("Unable to delete course."));
        }

        [Test]
        public async Task Post_ValidCourse_ReturnsOkResult()
        {
            // Arrange
            var validCourse = new Course { /* Initialize valid course properties */ };

            lmsServiceMock.Setup(service => service.AddCourse(validCourse))
                          .Returns(Task.CompletedTask);

            // Act
            var result = await lmsController.Post(validCourse);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.That(okResult.Value, Is.EqualTo("Course Added."));
        }

        [Test]
        public async Task Post_InvalidCourse_ReturnsBadRequest()
        {
            // Arrange
            var invalidCourse = new Course { /* Initialize invalid course properties */ };
            lmsController.ModelState.AddModelError("property", "error message");

            // Act
            var result = await lmsController.Post(invalidCourse);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Bad Request."));
        }

        [Test]
        public async Task Post_LMSServiceThrowsException_ReturnsBadRequest()
        {
            // Arrange
            var validCourse = new Course { /* Initialize valid course properties */ };

            lmsServiceMock.Setup(service => service.AddCourse(validCourse))
                          .Throws(new Exception("Sample exception message"));

            // Act
            var result = await lmsController.Post(validCourse);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.That(badRequestResult.Value, Is.EqualTo("Unable to add course."));
        }
    }
}
