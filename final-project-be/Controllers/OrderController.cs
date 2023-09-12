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
        public IActionResult Post([FromBody] OrderDTO orderDto)
        {
            if (orderDto == null)
                return BadRequest("Data should be inputed");

            Order order = new Order
            {
                Id = Guid.NewGuid(),
                id_user = Guid.NewGuid(),
                id_course = Guid.NewGuid(),
                schedule_date = orderDto.schedule_date
            };

            bool result = _orderDataAccess.Insert(order);

            if (result)
            {
                return StatusCode(201, order.Id);
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }


        }
    }
}