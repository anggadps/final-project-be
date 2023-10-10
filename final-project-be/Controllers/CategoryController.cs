using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using final_project_be.DTOs.Payment;

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

        [HttpGet("GetAllByAdmin")]
        public IActionResult GetAllByAdmin()
        {
            var categories = _categoryDataAccess.GetAllByAdmin();
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
        public async Task<IActionResult>  Post([FromForm] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Data should be inputed");

            if (categoryDto.ImageFile == null)
                return BadRequest("Image file should be provided");

            // Generate unique filename for the image
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + categoryDto.ImageFile.FileName;

            // Define the folder path where images will be saved
            string imagePath = Path.Combine("wwwroot/images", uniqueFileName);

            // Save the image file to the server
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await categoryDto.ImageFile.CopyToAsync(stream);
            }

            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name,
                Img = uniqueFileName,
                Description = categoryDto.Description,
                Is_active = true,
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
        public async Task<IActionResult> Put(Guid id, [FromForm] CategoryDTO categoryDto)
        {
            if (categoryDto == null)
                return BadRequest("Data should be inputed");

            /*if (categoryDto.ImageFile == null)
                return BadRequest("Image file should be provided");*/

            Category getExistingCategoryData = _categoryDataAccess.GetById(id);

            if(getExistingCategoryData == null)
                return NotFound();

            string uniqueFileName = getExistingCategoryData.Img;

            if(categoryDto.ImageFile != null )
            {
                // Generate unique filename for the image
                uniqueFileName = Guid.NewGuid().ToString() + "_" + categoryDto.ImageFile.FileName;

                // Define the folder path where images will be saved
                string imagePath = Path.Combine("wwwroot/images", uniqueFileName);

                // Save the image file to the server
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await categoryDto.ImageFile.CopyToAsync(stream);
                }

            }
            

            Category category = new Category
            {
                Id = Guid.NewGuid(),
                Name = categoryDto.Name,
                Img = uniqueFileName,
                Description = categoryDto.Description,
                Is_active = categoryDto.Is_active,
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
