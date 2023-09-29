namespace final_project_be.DTOs.OrderDetail
{
    public class OrderDetailDTO
    {
        public Guid Id_schedule { get; set; }
        public Guid Id_course { get; set; }
        public decimal Price { get; set; }
        public decimal Total_course { get; set; }
    }
}
