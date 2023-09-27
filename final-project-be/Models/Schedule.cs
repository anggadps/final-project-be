namespace final_project_be.Models
{
    public class Schedule
    {
        public Guid Id { get; set; }

        public string? Id_course { get; set; } = string.Empty;

        public DateTime Schedule_date { get; set; }

    }
}
