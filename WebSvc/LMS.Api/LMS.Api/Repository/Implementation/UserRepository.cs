using LMS.Api.Controllers;
using LMS.Api.Entities;
using LMS.Api.Models;
using LMS.Api.Repository.Interface;
using MongoDB.Driver;

namespace LMS.Api.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(UserRepository));
        private readonly IMongoDbContext dbContext;
        public UserRepository(IMongoDbContext _dbContext)
        {
            this.dbContext = _dbContext;
        }

        public Users GetUser(string username, string password)
        {
            try
            {
                return dbContext.users.Find(u => u.UserName == username && u.Password == password).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in GetUser: {ex}");
                return null;
            }
        }

        public async Task<string> Register(Users users)
        {
            try
            {
                await dbContext.users.InsertOneAsync(users);
                string insertedUserId = users.UserID;
                return insertedUserId;
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in RegisterUser: {ex}");
                return users.UserID;
            }
           
        }

        public async Task<string> ValidateUser(string userEmail, string userName)
        {
            try
            {
                var emailFilter = Builders<Users>.Filter.Eq(e => e.Email, userEmail);
                var userNameFilter = Builders<Users>.Filter.Eq(u => u.UserName, userName);
                var userExistsFilter = Builders<Users>.Filter.Or(emailFilter, userNameFilter);
                long existingUsersCount = await dbContext.users.CountDocumentsAsync(userExistsFilter);

                if (existingUsersCount > 0)
                {
                    return "invalidUser";
                }

                return "validUser";
            }
            catch (Exception ex)
            {
                // Log the exception
                logger.Error($"An error occurred in ValidateUser: {ex}");

                // You might return an error message or handle the exception in an appropriate way.
                return "error";
            }

        }
    }
}
