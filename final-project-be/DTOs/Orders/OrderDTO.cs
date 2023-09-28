namespace final_project_be.DTOs.Order
{
    public class OrderDTO
    {
        public string Id_schedule { get; set; } = string.Empty;
        public string Id_course { get; set; } = string.Empty;
        public int Price { get; set; }
    }
}