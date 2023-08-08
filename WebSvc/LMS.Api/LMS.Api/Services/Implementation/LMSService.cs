using LMS.Api.Models;
using LMS.Api.Repository.Interface;
using LMS.Api.Services.Contract;

namespace LMS.Api.Services.Implementation
{
    public class LMSService : ILMSService
    {
        private readonly ILMSRepository lmsRepository;

        public LMSService(ILMSRepository _lmsRepository)
        {
            this.lmsRepository = _lmsRepository;
        }
        public async Task AddCourse(Course course)
        {
            await lmsRepository.AddCourse(course);
        }
    }
}
