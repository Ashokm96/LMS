using LMS.Api.Models;

namespace LMS.Api.Repository.Interface
{
    public interface IUserRepository
    {
        Task<string> Register(Users users);
        Task<string> ValidateUser(string userEmail, string userLoginId);
        Users GetUser(string username, string password);


    }
}
