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
        private readonly CartDataAccess _cartDataAccess;
        public OrderController(OrderDataAccess orderDataAccess, OrderDetailDataAccess orderDetailDataAccess, CartDataAccess cartDataAccess)
        {
            _orderDataAccess = orderDataAccess;
            _orderDetailDataAccess = orderDetailDataAccess;
            _cartDataAccess = cartDataAccess;
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
                    Pay_date = DateTime.Now,

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

                    bool deleteCartResult = _cartDataAccess.DeleteCart(odto.Id_cart);

                    if (!deleteCartResult)
                    {
                        // Handle jika gagal menghapus Cart
                        return StatusCode(500, "Failed to remove item from cart.");
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


        [HttpGet("ViewInvoice")]
        public IActionResult ViewInvoice()
        {
            try
            {
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartList = _orderDataAccess.ViewInvoice(id);

                return Ok(cartList);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("ViewInvoiceDetail")]
        public IActionResult ViewInvoiceDetail(Guid id)
        {
            try
            {
                var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var invoiceDetailList = _orderDataAccess.ViewInvoiceDetail(id ,id_user);
                

                return Ok(invoiceDetailList);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet("ViewMyClass")]
        public IActionResult GetCartByIdUser()
        {
            try
            {
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartList = _orderDataAccess.ViewMyClass(id);

                return Ok(cartList);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


    }
}