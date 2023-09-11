namespace final_project_be.DTOs.Course
{
    public class CourseDTO
    {
        public Guid Id { get; }

        public string Name { get; set; } = string.Empty;

        public int Price { get; set; } = 0;

        public string? TypeCourse { get; set; } = string.Empty;

        public string? Img { get; set; }

    }
}