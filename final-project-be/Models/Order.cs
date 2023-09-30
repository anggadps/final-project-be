namespace final_project_be.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string No_invoice { get; set; } = string.Empty;
        public string? Id_user { get; set; }
        public string? Course_name { get; set; }
        public string? Category_name { get; set; }
        public DateTime Schedule_date  { get; set; }
        public decimal Price { get; set; }
        public decimal Total_course { get; set; }
        public decimal Total_price { get; set; }
        public DateTime Pay_date { get; set;}
        

    }
}