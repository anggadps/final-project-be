namespace final_project_be.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid Id_user_level { get; set; }
        public string Name { get; set; } = string.Empty;
        public string User_level { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool Is_active { get; set; }

    }
}