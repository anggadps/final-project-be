namespace final_project_be.DTOs.Invoice
{
    public class InvoiceDTO
    {
        public string id_user { get; set; } = string.Empty;
        public DateTime pay_date { get; }
        public DateTime schedule_date { get; set; }
        public string no_invoice { get; set; } = string.Empty;
        public int total_price { get; set; } = 0;
    }
}