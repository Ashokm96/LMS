using LMS.Api.Models;

namespace LMS.Api.Services.Contract
{
    public interface IUserService
    {
        Task<string> Register(Users users);
        Task<string> ValidateUser(string userEmail, string userLoginId);
        string Authenticate(string username, string password);


    }
}
