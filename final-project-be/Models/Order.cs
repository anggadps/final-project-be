namespace final_project_be.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid id_user { get; set; }
        public Guid id_course { get; set; }
        public DateTime schedule_date { get; set; }
    }
}