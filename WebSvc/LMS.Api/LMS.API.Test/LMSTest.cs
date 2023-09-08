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
            //Assert.IsInstanceOf<ConflictObjectResult>(result);
            //var conflictResult = result as ConflictObjectResult;
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

        //[Test]
        //public async Task Post_ValidCourse_ReturnsOkResult()
        //{
        //    // Arrange
        //    var validCourse = new Course { /* Initialize valid course properties */ };

        //    lmsServiceMock.Setup(service => service.AddCourse(validCourse))
        //                  .Returns(Task.CompletedTask);

        //    // Act
        //    var result = await lmsController.Post(validCourse);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult.Value, Is.EqualTo("Course Added."));
        //}

        //    [Test]
        //public async Task Post_InvalidCourse_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var invalidCourse = new Course { /* Initialize invalid course properties */ };
        //    lmsController.ModelState.AddModelError("property", "error message");

        //    // Act
        //    var result = await lmsController.Post(invalidCourse);

        //    // Assert
        //    Assert.IsInstanceOf<BadRequestObjectResult>(result);
        //    var badRequestResult = result as BadRequestObjectResult;
        //    Assert.That(badRequestResult.Value, Is.EqualTo("Bad Request."));
        //}

        //[Test]
        //public async Task Post_LMSServiceThrowsException_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var validCourse = new Course { /* Initialize valid course properties */ };

        //    lmsServiceMock.Setup(service => service.AddCourse(validCourse))
        //                  .Throws(new Exception("Sample exception message"));

        //    // Act
        //    var result = await lmsController.Post(validCourse);

        //    // Assert
        //    Assert.IsInstanceOf<BadRequestObjectResult>(result);
        //    var badRequestResult = result as BadRequestObjectResult;
        //    Assert.That(badRequestResult.Value, Is.EqualTo("Unable to add course."));
        //}

        //[Test]
        //public async Task GetCouseByDuration_ValidInput_ReturnsOkResult()
        //{
        //    // Arrange
        //    var technology = "SampleTechnology";
        //    var durationFromRange = 5;
        //    var durationToRange = 10;
        //    var expectedCourses = new List<Course> { /* Initialize expected course objects */ };

        //    lmsServiceMock.Setup(service => service.GetCouseByDuration(technology, durationFromRange, durationToRange))
        //                  .ReturnsAsync(expectedCourses);

        //    // Act
        //    var result = await lmsController.GetCouseByDuration(technology, durationFromRange, durationToRange);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult.Value, Is.EqualTo(expectedCourses));
        //}

        //[Test]
        //public async Task GetCouseByDuration_ServiceThrowsException_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var technology = "SampleTechnology";
        //    var durationFromRange = 5;
        //    var durationToRange = 10;

        //    lmsServiceMock.Setup(service => service.GetCouseByDuration(technology, durationFromRange, durationToRange))
        //                  .Throws(new Exception("Sample exception message"));

        //    // Act
        //    var result = await lmsController.GetCouseByDuration(technology, durationFromRange, durationToRange);

        //    // Assert
        //    Assert.IsInstanceOf<BadRequestObjectResult>(result);
        //    var badRequestResult = result as BadRequestObjectResult;
        //    Assert.That(badRequestResult.Value, Is.EqualTo("Unable to get course."));
        //}

        //[Test]
        //public async Task GetByCouseName_ValidTechnology_ReturnsOkResult()
        //{
        //    // Arrange
        //    var technology = "SampleTechnology";
        //    var expectedCourse = new Course { /* Initialize expected course object */ };

        //    lmsServiceMock.Setup(service => service.GetCourse(technology))
        //                  .ReturnsAsync(expectedCourse);

        //    // Act
        //    var result = await lmsController.GetByCouseName(technology);

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult.Value, Is.EqualTo(expectedCourse));
        //}

        //[Test]
        //public async Task GetByCouseName_ServiceThrowsException_ReturnsBadRequest()
        //{
        //    // Arrange
        //    var technology = "SampleTechnology";

        //    lmsServiceMock.Setup(service => service.GetCourse(technology))
        //                  .Throws(new Exception("Sample exception message"));

        //    // Act
        //    var result = await lmsController.GetByCouseName(technology);

        //    // Assert
        //    Assert.IsInstanceOf<BadRequestObjectResult>(result);
        //    var badRequestResult = result as BadRequestObjectResult;
        //    Assert.That(badRequestResult.Value, Is.EqualTo("Unable to get course."));
        //}

        //[Test]
        //public async Task Get_ValidRequest_ReturnsOkResult()
        //{
        //    // Arrange
        //    var expectedCourses = new List<Course> { /* Initialize expected course objects */ };

        //    lmsServiceMock.Setup(service => service.GetAllCourses())
        //                  .ReturnsAsync(expectedCourses);

        //    // Act
        //    var result = await lmsController.Get();

        //    // Assert
        //    Assert.IsInstanceOf<OkObjectResult>(result);
        //    var okResult = result as OkObjectResult;
        //    Assert.That(okResult.Value, Is.EqualTo(expectedCourses));
        //}

        //[Test]
        //public async Task Get_ServiceThrowsException_ReturnsBadRequest()
        //{
        //    // Arrange
        //    lmsServiceMock.Setup(service => service.GetAllCourses())
        //                  .Throws(new Exception("Sample exception message"));

        //    // Act
        //    var result = await lmsController.Get();

        //    // Assert
        //    Assert.IsInstanceOf<BadRequestObjectResult>(result);
        //    var badRequestResult = result as BadRequestObjectResult;
        //    Assert.That(badRequestResult.Value, Is.EqualTo("Unable to get all course."));
        //}
    }
}
