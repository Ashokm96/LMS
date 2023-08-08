using LMS.Api.Models;
using MongoDB.Driver;

namespace LMS.Api.Entities
{
    public interface IMongoDbContext
    {
        IMongoCollection<Users> users { get; }
        IMongoCollection<Course> course { get; }

    }
}
