using LMS.Api.Models;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        public ActionResult Post([FromBody] Users users)
        {
            if (users == null)
            {
                string message = string.Format("User details cannot be null.");
                return StatusCode((int)HttpStatusCode.BadRequest, message);
            }
            if (!users.Email.Contains("@") || !users.Email.Contains(".com"))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Invalid email format.");
            }
            if (!IsPasswordValid(users.Password))
            {
                return StatusCode((int)HttpStatusCode.BadRequest, "Password should be alphanumeric and at least 8 characters.");
            }
            //validate user details
            var userDetails = userService.ValidateUser(users.Email,users.UserName);
            if (userDetails.Result == "invalidUser")
            {
                return StatusCode((int)HttpStatusCode.Conflict, "User details already exists.");
            }
            if (userDetails.Result == "error")
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occured. contact your system administrator.");
            }
            //add new user
            userService.Register(users);
            if (users.UserID != null)
            {
                return StatusCode((int)HttpStatusCode.Created, "Registered succefully");
            }
            if (users.UserID == null)
            {
                return StatusCode((int)HttpStatusCode.Conflict, "Failed to register");
            }
            return StatusCode((int)HttpStatusCode.InternalServerError, "An error occured. contact your system administrator.");
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
        public ActionResult Login(Login login)
        {
            var token = userService.Authenticate(login.Username, login.Password);
            if (token == null)
            {
                return StatusCode((int)HttpStatusCode.NotFound, "user details not found.");
            }
            return StatusCode((int)HttpStatusCode.Created, new { Token = token });
        }

        #endregion
    }
}
