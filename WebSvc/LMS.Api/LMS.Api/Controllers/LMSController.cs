using LMS.Api.Models;
using LMS.Api.Repository.Implementation;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Mvc;

namespace LMS.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LMSController : ControllerBase
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(LMSController));
        private readonly ILMSService lmsService;
        public LMSController(ILMSService _lmsService)
        {
            this.lmsService = _lmsService;
        }

        [HttpPost]
        [Route("/api/v1.0/lms/courses/add")]
        public async Task<ActionResult> Post(Course course)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await lmsService.AddCourse(course);
                    return Ok("Course Added.");
                }
                catch (Exception ex)
                {
                    logger.Error($"An error occurred in AddCourse: {ex}.");
                    return BadRequest("Unable to add course.");
                }
            }
            else
            {
               return BadRequest("Bad Request.");
            }
        }
    }
}
