using LMS.Api.Models;

namespace LMS.Api.Repository.Interface
{
    public interface ILMSRepository
    {
        Task AddCourse(Course Course);
    }
}
