using LMS.Api.Controllers;
using LMS.Api.Models;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Mvc;
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
        private LMSController lmsController;

        [SetUp]
        public void Setup()
        {
            lmsServiceMock = new Mock<ILMSService>();
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
    }
}
