using final_project_be.Models;
using final_project_be.DataAccess;
using final_project_be.DTOs.Invoice;
using Microsoft.AspNetCore.Mvc;

namespace final_project_be.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceDataAccess _invoiceDataAccess;
        public InvoiceController(InvoiceDataAccess invoiceDataAccess)
        {
            _invoiceDataAccess = invoiceDataAccess;
        }


        // insert invoice
        [HttpPost]
        public IActionResult Post([FromBody] InvoiceDTO invoiceDto)
        {
            try
            {
                Invoice invoice = new Invoice
                {
                    Id = Guid.NewGuid(),
                    id_user = Guid.NewGuid(),
                    schedule_date = invoiceDto.schedule_date,
                    no_invoice = invoiceDto.no_invoice,
                    total_price = invoiceDto.total_price
                };

                InvoiceDetail invoiceDetail = new InvoiceDetail
                {
                    Id = Guid.NewGuid(),
                    id_invoice = invoice.Id,
                };

                bool result = _invoiceDataAccess.Insert(invoice, invoiceDetail);

                if (result)
                {
                    return StatusCode(201, invoice.Id);
                }
                else
                {
                    return StatusCode(500, "Internal server error");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }



    }
}