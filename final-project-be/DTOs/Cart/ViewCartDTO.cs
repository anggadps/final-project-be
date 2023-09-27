namespace final_project_be.DTOs.Cart
{
    public class ViewCartDTO
    {
        public Guid Id { get; set; }
        public Guid Id_schedule { get; set; }
        public string Category_name { get; set; } = string.Empty;
        public string Course_name { get; set;} = string.Empty;
        public decimal Price { get; set; }
        public DateTime Schedule_date { get; set; }
        public string? Img { get; set; }
    }
}
