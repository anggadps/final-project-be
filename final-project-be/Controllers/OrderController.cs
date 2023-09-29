using final_project_be.DataAccess;
using final_project_be.Models;
using final_project_be.DTOs.Order;
using Microsoft.AspNetCore.Mvc;
using final_project_be.DTOs.OrderDetail;
using System.Security.Claims;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class OrderController : ControllerBase
    {
        private readonly OrderDataAccess _orderDataAccess;
        private readonly OrderDetailDataAccess _orderDetailDataAccess;
        public OrderController(OrderDataAccess orderDataAccess, OrderDetailDataAccess orderDetailDataAccess)
        {
            _orderDataAccess = orderDataAccess;
            _orderDetailDataAccess = orderDetailDataAccess;
        }

        private string GenerateInvoiceNumber()
        {
            // Format nomor faktur: TANGGAL-WAKTU-RANDOM
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string randomPart = new Random().Next(1000, 9999).ToString(); // Angka acak 4 digit

            return $"{timestamp}-{randomPart}";
        }


        // insert order
        [HttpPost]
        public IActionResult Post([FromBody] OrderDetailDTO[] orderDetailDTO )
        {
            
            try
            {
                if (orderDetailDTO == null)
                    return BadRequest("Data should be inputed");

                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                decimal totalCourse = orderDetailDTO.Length;
                decimal totalPrice = 0;


                foreach (OrderDetailDTO price in orderDetailDTO)
                {
                    totalPrice += price.Price;
                }

                string invoiceNumber = GenerateInvoiceNumber();

                Order order = new Order
                {
                    Id = Guid.NewGuid(),
                    No_invoice = invoiceNumber,
                    Id_user = id,
                    Total_course = totalCourse,
                    Total_price = totalPrice,

                    

                };

                bool orderInsertResult = _orderDataAccess.Insert(order);

                if (!orderInsertResult)
                    return StatusCode(500, "Internal server error");


                foreach (OrderDetailDTO odto in orderDetailDTO)
                {

                    OrderDetail orderDetail = new OrderDetail
                    {
                        Id = Guid.NewGuid(),
                        Id_order = order.Id,
                        Id_course = odto.Id_course,
                        Id_schedule = odto.Id_schedule,
                    };

                    bool result = _orderDetailDataAccess.Insert(orderDetail);

                    if (!result)
                    {
                        // Jika gagal
                        // _orderDataAccess.Delete(order.Id);
                        return StatusCode(500, "Internal server error");
                    }
                }
                return StatusCode(201, "Order and OrderDetails inserted successfully");

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }






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