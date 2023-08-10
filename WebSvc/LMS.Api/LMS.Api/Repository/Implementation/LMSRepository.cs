using LMS.Api.Entities;
using LMS.Api.Models;
using LMS.Api.Repository.Interface;
using MongoDB.Driver;

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
                logger.Error($"An error occurred in add course: {ex}");
            }
        }

        public async Task<List<Course>> GetAllCourses()
        {
            try
            {
                return await dbContext.course.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get all courses: {ex}");
                return null;
            }
        }

        public async Task<Course> GetCourse(string course)
        {
            try
            {
                var course_filter = Builders<Course>.Filter.Eq(m => m.Name, course);
                return await dbContext.course.Find(course_filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get course: {ex}");
                return null;
            }
        }
    }
}
