using final_project_be.DataAccess;
using final_project_be.DTOs.Category;
using final_project_be.DTOs.ScheduleDTO;
using final_project_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleDataAccess _scheduleDataAccess;
        public ScheduleController(ScheduleDataAccess scheduleDataAccess)
        {
            _scheduleDataAccess = scheduleDataAccess;
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            var schedule = _scheduleDataAccess.GetById(id);

            if (schedule == null)
            {
                return NotFound("Data not found");
            }

            return Ok(schedule);
        }


        [HttpPost]
        public IActionResult Post([FromBody] ScheduleDTO scheduleDTO)
        {
            if (scheduleDTO == null)
                return BadRequest("Data should be inputed");

            Schedule schedule = new Schedule
            {
                Id = Guid.NewGuid(),
                Id_course = scheduleDTO.Id_course,
                Schedule_date = scheduleDTO.Schedule_date,
            };

            bool result = _scheduleDataAccess.AddSchedule(schedule);

            if (result)
            {
                return StatusCode(201, schedule.Id);
            }
            else
            {
                return StatusCode(500, "Error occur");
            }
        }
    }
}
