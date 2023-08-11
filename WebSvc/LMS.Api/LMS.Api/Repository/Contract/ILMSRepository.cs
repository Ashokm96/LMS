using LMS.Api.Models;

namespace LMS.Api.Repository.Interface
{
    public interface ILMSRepository
    {
        Task AddCourse(Course Course);
        Task<List<Course>> GetAllCourses();
        Task<Course> GetCourse(string course);
        Task<List<Course>> GetCouseByDuration(string course, int durationFromRange, int durationToRange);
        Task DeleteCourse(string courseName);

    }
}
