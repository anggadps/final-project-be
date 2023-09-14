namespace final_project_be.Models
{
    public class Course
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public string? id_category { get; set; } = string.Empty;
        public string? Img { get; set; }

    }
}
