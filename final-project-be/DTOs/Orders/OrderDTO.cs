namespace final_project_be.DTOs.Order
{
    public class OrderDTO
    {
        public string id_user { get; set; } = string.Empty;
        public string id_course { get; set; } = string.Empty;
        public DateTime schedule_date { get; set; }
    }
}