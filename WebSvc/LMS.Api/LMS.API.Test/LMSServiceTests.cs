using NUnit.Framework;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using LMS.Api.Models;
using LMS.Api.Services.Implementation;
using LMS.Api.Repository.Interface;
using System.Diagnostics.CodeAnalysis;

namespace LMS.API.Test
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class LMSServiceTests
    {
        private LMSService lmsService;
        private Mock<ILMSRepository> lmsRepositoryMock;

        [SetUp]
        public void Setup()
        {
            lmsRepositoryMock = new Mock<ILMSRepository>();
            lmsService = new LMSService(lmsRepositoryMock.Object);
        }

        [Test]
        public async Task AddCourse_Should_ReturnAddedCourse()
        {
            // Arrange
            var course = new Course { Name = "TestCourse" };
            lmsRepositoryMock.Setup(repo => repo.AddCourse(course)).ReturnsAsync(course);

            // Act
            var result = await lmsService.AddCourse(course);

            // Assert
            Assert.AreEqual(course, result);
        }

        [Test]
        public async Task DeleteCourse_Should_ReturnTrue()
        {
            // Arrange
            string courseName = "TestCourse";
            lmsRepositoryMock.Setup(repo => repo.DeleteCourse(courseName)).ReturnsAsync(true);

            // Act
            var result = await lmsService.DeleteCourse(courseName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public async Task GetAllCourses_Should_ReturnListOfCourses()
        {
            // Arrange
            var courses = new List<Course> { new Course(), new Course() };
            lmsRepositoryMock.Setup(repo => repo.GetAllCourses()).ReturnsAsync(courses);

            // Act
            var result = await lmsService.GetAllCourses();

            // Assert
            CollectionAssert.AreEqual(courses, result);
        }

        [Test]
        public async Task GetCourse_Should_ReturnCourse()
        {
            // Arrange
            string courseName = "TestCourse";
            var course = new Course { Name = courseName };
            lmsRepositoryMock.Setup(repo => repo.GetCourse(courseName)).ReturnsAsync(course);

            // Act
            var result = await lmsService.GetCourse(courseName);

            // Assert
            Assert.AreEqual(course, result);
        }

        [Test]
        public async Task GetCourseByDuration_Should_ReturnListOfCourses()
        {
            // Arrange
            string courseName = "TestCourse";
            int durationFromRange = 10;
            int durationToRange = 20;
            var courses = new List<Course> { new Course(), new Course() };
            lmsRepositoryMock.Setup(repo => repo.GetCouseByDuration(courseName, durationFromRange, durationToRange)).ReturnsAsync(courses);

            // Act
            var result = await lmsService.GetCouseByDuration(courseName, durationFromRange, durationToRange);

            // Assert
            CollectionAssert.AreEqual(courses, result);
        }
    }
}
