using LMS.Api.Models;
using LMS.Api.Services.Contract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
                return StatusCode((int)HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in getall courses: {ex}.");
                return StatusCode((int)HttpStatusCode.Conflict, "Unable to get courses.");
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
                if (res==null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, "No course details found.");
                }
                return StatusCode((int)HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get by couse name: {ex}.");
                return StatusCode((int)HttpStatusCode.Conflict, "Unable to get course.");
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
                if (res == null)
                {
                    return StatusCode((int)HttpStatusCode.NotFound, "No course details found.");
                }
                return StatusCode((int)HttpStatusCode.OK, res);
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in get by couse name: {ex}.");
                return StatusCode((int)HttpStatusCode.Conflict, "Unable to get course.");
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
                    var result = await lmsService.AddCourse(course);
                    if (result.CourseID == null)
                    {
                        return StatusCode((int)HttpStatusCode.Conflict, "Unable to add course.");
                    }
                    return StatusCode((int)HttpStatusCode.Created, "Course Added.");
                }
                catch (Exception ex)
                {
                    logger.Error($"An error occurred in add course: {ex}.");
                    return StatusCode((int)HttpStatusCode.Conflict, "Unable to add course.");
                }
            }
            else
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occured. contact your system administrator.");
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
                    return StatusCode((int)HttpStatusCode.BadRequest, "Course details not found!");
                }
                var response = await lmsService.DeleteCourse(coursename);
                if (response)
                {
                    return StatusCode((int)HttpStatusCode.OK, "Course deleted Successfully");
                }
                return StatusCode((int)HttpStatusCode.Conflict, "Unable to delete course.");
            }
            catch (Exception ex)
            {
                logger.Error($"An error occurred in delete couse: {ex}.");
                return StatusCode((int)HttpStatusCode.InternalServerError, "An error occured. contact your system administrator.");
            }
        }
    }
}
