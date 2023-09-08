using LMS.Api.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace LMS.Api.Entities
{
    [ExcludeFromCodeCoverage]
    public class MongoDbContext : IMongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IOptions<DBConfiguration> options)
        {
            var client = new MongoClient(options.Value.ConnectionString);
            _database = client.GetDatabase(options.Value.DatabaseName);
        }
        public IMongoCollection<Users> users => _database.GetCollection<Users>("users");
        public IMongoCollection<Course> course => _database.GetCollection<Course>("courses");

    }
}
