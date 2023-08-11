using LMS.Api.Models;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// get all courses
        /// </summary>
        [HttpGet]
        [Route("/api/v1.0/lms/courses/getall")]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var res = await lmsService.GetAllCourses();
                return Ok(res);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in getall courses: {ex}.");
                return BadRequest("Unable to get all course.");
            }
        }

        /// <summary>
        /// get course by technology
        /// </summary>
        [HttpGet]
        [Route("/api/v1.0/lms/courses/info/{technology}")]
        [Authorize]
        public async Task<IActionResult> GetByCouseName(string technology)
        {
            try
            {
                var res = await lmsService.GetCourse(technology);
                return Ok(res);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get by couse name: {ex}.");
                return BadRequest("Unable to get course.");
            }
        }

        /// <summary>
        /// get course by technology and duration
        /// </summary>
        [HttpGet]
        [Route("/api/v/1.0/lms/courses/get/{technology}/{durationFromRange}/{durationToRange}")]
        [Authorize]
        public async Task<IActionResult> GetCouseByDuration(string technology,int durationFromRange,int durationToRange)
        {
            try
            {
                var res = await lmsService.GetCouseByDuration(technology,durationFromRange,durationToRange);
                return Ok(res);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get by couse name: {ex}.");
                return BadRequest("Unable to get course.");
            }
        }

        /// <summary>
        /// add new course
        /// </summary>
        [HttpPost]
        [Route("/api/v1.0/lms/courses/add")]
        [Authorize(Policy = "AdminPolicy")]
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
                    logger.Error($"An error occurred in add course: {ex}.");
                    return BadRequest("Unable to add course.");
                }
            }
            else
            {
               return BadRequest("Bad Request.");
            }
        }

        /// <summary>
        /// delete course by name
        /// </summary>
        [HttpDelete]
        [Route("/api/v1.0/lms/courses/delete/{coursename}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteByCouseName(string coursename)
        {
            try
            {
                var res = await lmsService.GetCourse(coursename);
                if (res == null)
                {
                    return BadRequest("Course details not found!"); 
                }
                await lmsService.DeleteCourse(coursename);
                return Ok("Course deleted Successfully");
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in delete couse: {ex}.");
                return BadRequest("Unable to delete course.");
            }
        }
    }
}
