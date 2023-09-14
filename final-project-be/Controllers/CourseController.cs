using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseDataAccess _courseDataAccess;
        public CourseController(CourseDataAccess courseDataAccess)
        {
            _courseDataAccess = courseDataAccess;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseDataAccess.GetAll();
            return Ok(courses);
        }

        // get by id
        [HttpGet("GetById")]
        public IActionResult GetById(Guid id)
        {
            Course? course = _courseDataAccess.GetById(id);

            if (course == null)
            {
                return NotFound("Data not found");
            }

            return Ok(course);
        }

        // get by name
        [Authorize]
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            Course? course = _courseDataAccess.GetByName(name);

            if (course == null)
            {
                return NotFound("Data not found");
            }

            return Ok(course);
        }

        // post
        [HttpPost]
        public IActionResult Post([FromBody] CourseDTO courseDto)
        {
            if (courseDto == null)
                return BadRequest("Data should be inputed");

            Course course = new Course
            {
                Id = Guid.NewGuid(),
                Name = courseDto.Name,
                Price = courseDto.Price,
                id_category = courseDto.id_category,
                Img = courseDto.Img,
            };

            bool result = _courseDataAccess.Insert(course);

            if (result)
            {
                return StatusCode(201, course.Id);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        // put
        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] CourseDTO courseDTO)
        {
            if (courseDTO == null)
                return BadRequest("Data should be inputed");

            Course course = new Course
            {
                Id = Guid.NewGuid(),
                Name = courseDTO.Name,
                Price = courseDTO.Price,
                id_category = courseDTO.id_category,
                Img = courseDTO.Img,
            };

            bool result = _courseDataAccess.Update(id, course);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Error occur");
            }
        }

        // delete
        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            bool result = _courseDataAccess.Delete(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Error occur");
            }
        }
    }
}