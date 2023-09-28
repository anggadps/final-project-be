namespace final_project_be.Models
{
    public class Payment
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Logo { get; set; } = string.Empty;
        public Boolean Is_active { get; set; }
    }
}
