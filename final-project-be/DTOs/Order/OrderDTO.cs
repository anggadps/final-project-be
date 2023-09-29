namespace final_project_be.DTOs.Order
{
    public class OrderDTO
    {
        public string? No_invoice { get; set; }
        public string? Id_user { get; set; }
        public int Total_course { get; set; }
        public int Total_price { get; set; }
    }
}