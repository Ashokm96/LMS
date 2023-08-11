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

        public async Task DeleteCourse(string courseName)
        {
            await lmsRepository.DeleteCourse(courseName);
        }

        public async Task<List<Course>> GetAllCourses()
        {
           return  await lmsRepository.GetAllCourses();
        }

        public async Task<Course> GetCourse(string course)
        {
            return await lmsRepository.GetCourse(course);
        }


        public async Task<List<Course>> GetCouseByDuration(string course, int durationFromRange, int durationToRange)
        {
            return await lmsRepository.GetCouseByDuration(course,durationFromRange,durationToRange);
        }
    }
}
