﻿using final_project_be.DataAccess;
using final_project_be.DTOs.Cart;
using final_project_be.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartDataAccess _cartDataAccess;

        public CartController(CartDataAccess cartDataAccess)
        {
            _cartDataAccess = cartDataAccess;
        }


        [HttpPost("AddCart")]
        public IActionResult AddCart([FromBody] CartDTO cartDTO)
        {
            try
            {
                var id_user = User.FindFirstValue(ClaimTypes.NameIdentifier);

                Cart? existingCart = _cartDataAccess.CheckCart(id_user, cartDTO.Id_schedule);

                if (existingCart != null)
                {
                    return BadRequest("Jadwal ini sudah ada di dalam keranjang Anda.");
                }

                Cart cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    Id_user = id_user,
                    Id_schedule = cartDTO.Id_schedule

                };

                bool result = _cartDataAccess.AddCart(cart);

                if (result)
                {
                    return StatusCode(201, cartDTO);
                }
                else
                {
                    return StatusCode(500, "Data not inserted");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


        [HttpGet("GetCartByIdUser")]
        public IActionResult GetCartByIdUser()
        {
            try
            {
                var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var cartList = _cartDataAccess.GetViewCart(id);

                return Ok(cartList);
            }
            catch (Exception ex)
            {
               return Problem(ex.Message);
            }
        }

        [HttpDelete]
        public IActionResult DeleteCart(Guid id)
        {
            bool result = _cartDataAccess.DeleteCart(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
