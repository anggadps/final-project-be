using final_project_be.DataAccess;
using final_project_be.DTOs.Course;
using final_project_be.DTOs.Payment;
using final_project_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentDataAccess _paymentDataAccess;
        public PaymentController(PaymentDataAccess paymentDataAccess)
        {
            _paymentDataAccess = paymentDataAccess;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] PaymentDTO paymentDTO)
        {
            if (paymentDTO == null)
                return BadRequest("Data should be inputed");

            if (paymentDTO.ImageFile == null)
                return BadRequest("Image file should be provided");

            // Generate unique filename for the image
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + paymentDTO.ImageFile.FileName;

            // Define the folder path where images will be saved
            string imagePath = Path.Combine("wwwroot/images", uniqueFileName);

            // Save the image file to the server
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await paymentDTO.ImageFile.CopyToAsync(stream);
            }

            Payment payment = new Payment
            {
                Id = Guid.NewGuid(),
                Name = paymentDTO.Name,
                Logo = uniqueFileName,
                Is_active = true,
            };

            bool result = _paymentDataAccess.Insert(payment);

            if (result)
            {
                return StatusCode(201, payment.Id);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Guid id, [FromForm] PaymentDTO paymentDTO)
        {
            if (paymentDTO == null)
                return BadRequest("Data should be inputed");

            if (paymentDTO.ImageFile == null)
                return BadRequest("Image file should be provided");

            // Generate unique filename for the image
            string uniqueFileName = Guid.NewGuid().ToString() + "_" + paymentDTO.ImageFile.FileName;

            // Define the folder path where images will be saved
            string imagePath = Path.Combine("wwwroot/images", uniqueFileName);

            // Save the image file to the server
            using (var stream = new FileStream(imagePath, FileMode.Create))
            {
                await paymentDTO.ImageFile.CopyToAsync(stream);
            }

            Payment payment = new Payment
            {
                Id = id,
                Name = paymentDTO.Name,
                Logo = uniqueFileName,
                Is_active = true,

            };

            bool result = _paymentDataAccess.Update(id, payment);


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
