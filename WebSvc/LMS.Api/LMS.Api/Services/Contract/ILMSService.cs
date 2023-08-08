using LMS.Api.Models;

namespace LMS.Api.Services.Contract
{
    public interface ILMSService
    {
        Task AddCourse(Course course);
    }
}
