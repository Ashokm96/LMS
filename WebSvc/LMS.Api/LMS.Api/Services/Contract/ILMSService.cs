using LMS.Api.Models;

namespace LMS.Api.Services.Contract
{
    public interface ILMSService
    {
        Task<Course> AddCourse(Course course);
        Task<List<Course>> GetAllCourses();
        Task<List<Course>> GetCourse(string course);
        Task<List<Course>> GetCouseByDuration(string course, int durationFromRange, int durationToRange);
        Task<Boolean> DeleteCourse(string courseName);

    }
}
