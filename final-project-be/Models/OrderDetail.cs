namespace final_project_be.Models
{
    public class OrderDetail
    {
        public Guid Id { get; set; }
        public Guid Id_order { get; set; }
        public Guid Id_schedule { get; set; }
        public Guid Id_course { get; set; }
    }
}
