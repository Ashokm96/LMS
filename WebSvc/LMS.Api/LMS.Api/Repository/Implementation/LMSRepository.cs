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
        public async Task<Course> AddCourse(Course course)
        {
            try
            {
                await dbContext.course.InsertOneAsync(course);
                return course;
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in add course: {ex}");
                return course;
            }
        }

        public async Task<Boolean> DeleteCourse(string courseName)
        {
            try
            {
               var res = await dbContext.course.DeleteOneAsync(x => x.Technology == courseName);
               if (res.DeletedCount>0 && res.IsAcknowledged)
               {
                    return true;
               }
               return false;
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in delete course: {ex}");
                return false;
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
                var course_filter = Builders<Course>.Filter.Eq(m => m.Technology, course);
                return await dbContext.course.Find(course_filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get course: {ex}");
                return null;
            }
        }

        public async Task<List<Course>> GetCouseByDuration(string course, int durationFromRange, int durationToRange)
        {
            try
            {
                var filter = Builders<Course>.Filter.And(
                 Builders<Course>.Filter.Gte(course => course.Duration, durationFromRange),
                 Builders<Course>.Filter.Lte(course => course.Duration, durationToRange));

                var courses = dbContext.course.Find(filter).ToList();
                return courses;
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get couse by duration: {ex}");
                return new List<Course>();
            }
            
        }
    }
}
