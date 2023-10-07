﻿using final_project_be.DataAccess;
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


        [HttpGet]
        public IActionResult GetAll()
        {
            var payments = _paymentDataAccess.GetAll();
            return Ok(payments);
        }

        [HttpGet("GetAllByAdmin")]
        public IActionResult GetAllByAdmin()
        {
            var payments = _paymentDataAccess.GetAllByAdmin();
            return Ok(payments);
        }

        [HttpGet("GetById")]
        public IActionResult Get(Guid id)
        {
            Payment? payment = _paymentDataAccess.GetById(id);

            if (payment == null)
            {
                return NotFound("Data not found");
            }

            return Ok(payment);
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

            /*if (paymentDTO.ImageFile == null)
                return BadRequest("Image file should be provided");*/

            Payment getExistingPaymentData = _paymentDataAccess.GetById(id);

            if (getExistingPaymentData == null)
                return NotFound();

            string uniqueFileName = getExistingPaymentData.Logo;

            if (paymentDTO.ImageFile != null)
            {
                // Generate unique filename for the image
                uniqueFileName = Guid.NewGuid().ToString() + "_" + paymentDTO.ImageFile.FileName;

                // Define the folder path where images will be saved
                string imagePath = Path.Combine("wwwroot/images", uniqueFileName);

                // Save the image file to the server
                using (var stream = new FileStream(imagePath, FileMode.Create))
                {
                    await paymentDTO.ImageFile.CopyToAsync(stream);
                }

            }

            Payment payment = new Payment
            {
                Id = id,
                Name = paymentDTO.Name,
                Logo = uniqueFileName,
                Is_active = paymentDTO.Is_active,
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
