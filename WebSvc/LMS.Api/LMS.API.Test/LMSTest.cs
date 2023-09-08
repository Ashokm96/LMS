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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LMS.API.Test
{
    [ExcludeFromCodeCoverage]
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
        public async Task DeleteByCouseName_ValidCourseName_ReturnsOk()
        {
            // Arrange
            string validCourseName = "ValidCourse";
            lmsServiceMock.Setup(x => x.GetCourse(validCourseName)).ReturnsAsync(new Course());
            lmsServiceMock.Setup(x => x.DeleteCourse(validCourseName)).ReturnsAsync(true);

            // Act
            var result = await lmsController.DeleteByCouseName(validCourseName) as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual("Course deleted Successfully", result.Value);
        }

        [Test]
        public async Task DeleteByCouseName_DeleteFailed_ReturnsConflict()
        {
            // Arrange
            string validCourseName = "ValidCourse";
            lmsServiceMock.Setup(x => x.GetCourse(validCourseName)).ReturnsAsync(new Course());
            lmsServiceMock.Setup(x => x.DeleteCourse(validCourseName)).ReturnsAsync(false);

            // Act
            var result = await lmsController.DeleteByCouseName(validCourseName) as ObjectResult; 

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
            Assert.AreEqual("Unable to delete course.", result.Value);
        }

        [Test]
        public async Task DeleteByCouseName_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            string validCourseName = "ValidCourse";
            lmsServiceMock.Setup(x => x.GetCourse(validCourseName)).ThrowsAsync(new Exception("Some error message"));

            // Act
            IActionResult result = await lmsController.DeleteByCouseName(validCourseName);

            // Assert
            Assert.IsInstanceOf<ObjectResult>(result);
            var internalServerErrorResult = result as ObjectResult;
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, internalServerErrorResult.StatusCode);
            Assert.AreEqual("An error occured. contact your system administrator.", internalServerErrorResult.Value);
        }

        [Test]
        public async Task GetCouseByDuration_ValidInput_ReturnsOk()
        {
            // Arrange
            string technology = "Technology";
            int durationFromRange = 1;
            int durationToRange = 10;
            // Mock the response from your service
            Course course = new Course()
            {
                CourseID = "1",
                Description = "",
                Duration = 1,
                LaunchUrl = "www.microsoft.com",
                Name = "c#",
                Technology = ".net"
            };
            List<Course> listOfCourses = new List<Course>();
            listOfCourses.Add(course);
            
            lmsServiceMock.Setup(x => x.GetCouseByDuration(technology, durationFromRange, durationToRange)).ReturnsAsync(listOfCourses);

            // Act
            var result = await lmsController.GetCouseByDuration(technology, durationFromRange, durationToRange) as ObjectResult; 

            // Assert
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(listOfCourses, result.Value);
        }

        [Test]
        public async Task GetCouseByDuration_NoCourseFound_ReturnsNotFound()
        {
            // Arrange
            string technology = "Technology";
            int durationFromRange = 1;
            int durationToRange = 10;
            // Mock the service to return null
            lmsServiceMock.Setup(x => x.GetCouseByDuration(technology, durationFromRange, durationToRange)).ReturnsAsync(new List<Course>());

            // Act
            var result = await lmsController.GetCouseByDuration(technology, durationFromRange, durationToRange) as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
            Assert.AreEqual("No course details found.", result.Value);
        }

        [Test]
        public async Task GetCouseByDuration_ExceptionThrown_ReturnsConflict()
        {
            // Arrange
            string technology = "Technology";
            int durationFromRange = 1;
            int durationToRange = 10;
            // Mock the service to throw an exception
            lmsServiceMock.Setup(x => x.GetCouseByDuration(technology, durationFromRange, durationToRange)).ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await lmsController.GetCouseByDuration(technology, durationFromRange, durationToRange) as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
            Assert.AreEqual("Unable to get course.", result.Value);
        }

        [Test]
        public async Task Get_SuccessfulRequest_ReturnsOk()
        {
            // Arrange
            // Mock the response from your service
            Course course = new Course()
            {
                CourseID = "1",
                Description = "",
                Duration = 1,
                LaunchUrl = "www.microsoft.com",
                Name = "c#",
                Technology = ".net"
            };
            List<Course> listOfCourses = new List<Course> { course };
            lmsServiceMock.Setup(x => x.GetAllCourses()).ReturnsAsync(listOfCourses);

            // Act
            var result = await lmsController.Get() as ObjectResult;

            // Assert
            Assert.IsNotNull(result); 
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(listOfCourses, result.Value);
        }

        [Test]
        public async Task Get_ExceptionThrown_ReturnsConflict()
        {
            // Arrange
            // Mock the service to throw an exception
            lmsServiceMock.Setup(x => x.GetAllCourses()).ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await lmsController.Get() as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
            Assert.AreEqual("Unable to get courses.", result.Value);
        }

        [Test]
        public async Task Post_ValidCourse_ReturnsCreated()
        {
            // Arrange
            var course = new Course
            {
                Description = "",
                Duration = 1,
                LaunchUrl = "www.microsoft.com",
                Name = "c#",
                Technology = ".net"
            };
            // Mock the response from your service
            var expectedResult = new Course
            {
                CourseID = "1",
                Description = "",
                Duration = 1,
                LaunchUrl = "www.microsoft.com",
                Name = "c#",
                Technology = ".net"
            };
            lmsServiceMock.Setup(x => x.AddCourse(course)).ReturnsAsync(expectedResult);

            // Act
            var result = await lmsController.Post(course) as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Created, result.StatusCode);
            Assert.AreEqual("Course Added.", result.Value);
        }

        [Test]
        public async Task Post_AddCourseFailed_ReturnsConflict()
        {
            // Arrange
            var course = new Course
            {
                Description = "",
                Duration = 1,
                LaunchUrl = "www.microsoft.com",
                Name = "c#",
                Technology = ".net"
            };
            // Mock the service to return a result with a null CourseID, indicating failure
            var expectedResult = new Course
            {
                CourseID = null,
                Description = "",
                Duration = 1,
                LaunchUrl = "www.microsoft.com",
                Name = "c#",
                Technology = ".net"
            };
            lmsServiceMock.Setup(x => x.AddCourse(course)).ReturnsAsync(expectedResult);

            // Act
            var result = await lmsController.Post(course) as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
            Assert.AreEqual("Unable to add course.", result.Value);
        }

        [Test]
        public async Task Post_InvalidModel_ReturnsInternalServerError()
        {
            // Arrange
            // Create an invalid model (e.g., missing required fields) that would fail ModelState.IsValid
            var invalidCourse = new Course();
            lmsController.ModelState.AddModelError("PropertyName", "Error message"); // Add model errors as needed

            // Act
            var result = await lmsController.Post(invalidCourse) as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.InternalServerError, result.StatusCode);
            Assert.AreEqual("An error occured. contact your system administrator.", result.Value);
        }

        [Test]
        public async Task Post_ExceptionThrown_ReturnsConflict()
        {
            // Arrange
            var course = new Course
            {
                // Initialize course properties
            };

            // Mock the service to throw an exception
            lmsServiceMock.Setup(x => x.AddCourse(course)).ThrowsAsync(new Exception("Some error message"));

            // Act
            var result = await lmsController.Post(course) as ObjectResult;

            // Assert
            Assert.AreEqual((int)HttpStatusCode.Conflict, result.StatusCode);
            Assert.AreEqual("Unable to add course.", result.Value);
        }

    }
}
