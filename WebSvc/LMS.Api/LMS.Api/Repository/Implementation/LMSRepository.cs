using LMS.Api.Entities;
using LMS.Api.Models;
using LMS.Api.Repository.Interface;

namespace LMS.Api.Repository.Implementation
{
    public class LMSRepository : ILMSRepository
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LMSRepository));
        private readonly IMongoDbContext dbContext;
        public LMSRepository(IMongoDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task AddCourse(Course course)
        {
            try
            {
                await dbContext.course.InsertOneAsync(course);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in AddCourse: {ex}");
            }
        }
    }
}
