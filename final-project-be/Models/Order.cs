namespace final_project_be.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public string? Id_user { get; set; }
        public string? Id_schedule { get; set; }
        public string? Id_course { get; set; }
        public int Total_price { get; set; }
        

    }
}