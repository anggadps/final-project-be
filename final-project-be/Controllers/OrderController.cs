using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.Order;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly OrderDataAccess _orderDataAccess;
        public OrderController(OrderDataAccess orderDataAccess)
        {
            _orderDataAccess = orderDataAccess;
        }

        // insert order
        [HttpPost]
        public IActionResult Post([FromBody] OrderDTO[] orderDto)
        {
            if (orderDto == null)
                return BadRequest("Data should be inputed");


            foreach(OrderDTO odto in orderDto) 
            { 
                Console.WriteLine(odto.Id_course);
                Console.WriteLine(odto.Id_schedule);
                Console.WriteLine(odto.Price);
                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    Id_course = odto.Id_course,
                    Id_schedule = odto.Id_schedule,
                    Price = odto.Price,
                };

                bool result = _orderDataAccess.Insert(order);
            }
            return StatusCode(500, "Internal server error");

            /*if (result)
            {
                return StatusCode(201, order.Id);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }*/


        }
    }
}