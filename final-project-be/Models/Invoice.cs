namespace final_project_be.Models
{
    public class Invoice
    {
        public Guid Id { get; set; }
        public Guid id_user { get; set; }
        public DateTime pay_date { get; }
        public DateTime schedule_date { get; set; }
        public string no_invoice { get; set; } = string.Empty;
        public int total_price { get; set; } = 0;
    }
}