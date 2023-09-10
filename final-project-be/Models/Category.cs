namespace final_project_be.Models
{
    public class Category
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Img { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
