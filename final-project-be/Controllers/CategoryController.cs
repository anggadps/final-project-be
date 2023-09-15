using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryDataAccess _categoryDataAccess;
        public CategoryController(CategoryDataAccess categoryDataAccess)
        {
            _categoryDataAccess = categoryDataAccess;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var categories = _categoryDataAccess.GetAll();
            return Ok(categories);
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            Category? category = _categoryDataAccess.GetById(id);

            if (category == null)
            {
                return NotFound("Data not found");
            }

            return Ok(category);
        }


        [HttpPost]
        public IActionResult Post([FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Data should be inputed");

            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name,
                Img = categoryDto.Img,
                Description = categoryDto.Description,
            };

            bool result = _categoryDataAccess.Insert(category);

            if (result)
            {
                return StatusCode(201, category.Id);
            }
            else
            {
                return StatusCode(500, "Error occur");
            }
        }


        [HttpPut]
        public IActionResult Put(Guid id, [FromBody] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Data should be inputed");

            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name,
                Img = categoryDto.Img,
                Description = categoryDto.Description
            };

            bool result = _categoryDataAccess.Update(id, category);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Error occur");
            }
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            bool result = _categoryDataAccess.Delete(id);

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
