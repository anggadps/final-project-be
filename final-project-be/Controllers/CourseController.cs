using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.Course;
using Microsoft.AspNetCore.Mvc;
using final_project_be.DTOs.Payment;

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
            // test merge
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = _courseDataAccess.GetAll();
            return Ok(courses);
        }

        // get by id_category
        [HttpGet("GetByIdCategory")]
        public IActionResult GetByIdCategory(Guid id)
        {
            List<CourseByCategory> coursesByCategory = _courseDataAccess.GetByIdCategory(id);

            if (coursesByCategory == null || coursesByCategory.Count == 0)
            {
                return NotFound("Data not found");
            }

            return Ok(coursesByCategory);
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
        public async Task<IActionResult> Post([FromForm] CourseDTO courseDto)
        {
            if (courseDto == null)
                return BadRequest("Data should be inputed");

            if (courseDto.ImageFile == null)
                return BadRequest("Image file should be provided");

            // Generate unique filename for the image
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + courseDto.ImageFile.FileName;

            // Define the folder path where images will be saved
            string imagePath = Path.Combine("wwwroot/images", uniqueFileName);

            // Save the image file to the server
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await courseDto.ImageFile.CopyToAsync(stream);
            }

            Course course = new Course
            {
                Id = Guid.NewGuid(),
                Name = courseDto.Name,
                Price = courseDto.Price,
                id_category = courseDto.id_category,
                Img = uniqueFileName,
                Description = courseDto.Description,
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
        public async Task<IActionResult>  Put(Guid id, [FromForm] CourseDTO courseDTO)
        {
            if (courseDTO == null)
                return BadRequest("Data should be inputed");

            if (courseDTO.ImageFile == null)
                return BadRequest("Image file should be provided");

            // Generate unique filename for the image
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + courseDTO.ImageFile.FileName;

            // Define the folder path where images will be saved
            string imagePath = Path.Combine("wwwroot/images", uniqueFileName);

            // Save the image file to the server
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await courseDTO.ImageFile.CopyToAsync(stream);
            }

            Course course = new Course
            {
                Id = Guid.NewGuid(),
                Name = courseDTO.Name,
                Price = courseDTO.Price,
                id_category = courseDTO.id_category,
                Img = uniqueFileName
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