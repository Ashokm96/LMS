using LMS.Api.Models;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/v1.0/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;
        public UserController(IUserService _userService)
        {
            this.userService = _userService;
        }

        #region Register user

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1.0/lms/user/register")]
        public ActionResult<Result<string>> Post([FromBody] Users users)
        {
            Result<string> result = new Result<string>();
            if (users == null)
            {
                string message = string.Format("User details cannot be null.");
                result.ErrorMessage = message;
                return result;
            }
            if (!users.Email.Contains("@") || !users.Email.Contains(".com"))
            {
                return BadRequest("Invalid email format.");
            }
            if (!IsPasswordValid(users.Password))
            {
                return BadRequest("Password should be alphanumeric and at least 8 characters.");
            }
            //validate user details
            var userDetails = userService.ValidateUser(users.Email,users.UserName);
            if (userDetails.Result == "invalidUser")
            {
                return BadRequest("User details already exists.");
            }
            if (userDetails.Result == "error")
            {
                return BadRequest("An error occured. contact your system administrator.");
            }
            //add new user
            userService.Register(users);
            if (users.UserID != null)
            {
                result.Data = users.UserID;
                result.Message = "Registered succefully";
                return result;
            }
            if (users.UserID == null)
            {
                return BadRequest("Failed to register");
            }
            return result;
        }

        private bool IsPasswordValid(string password)
        {
            // Password should be alphanumeric and at least 8 characters.
            return !string.IsNullOrWhiteSpace(password) && password.Length >= 8 && password.All(char.IsLetterOrDigit);
        }

        #endregion

        #region validate user

        /// <summary>
        /// Validate user
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/v1.0/lms/user/login")]
        [AllowAnonymous]
        public ActionResult<string> Login(Login login)
        {
            var token = userService.Authenticate(login.Username, login.Password);
            if (token == null)
            {
                return BadRequest("unauthorized user");
            }
            return Ok(new { Token = token });
        }

        #endregion
    }
}
