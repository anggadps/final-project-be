namespace final_project_be.DTOs.User
{
    public class UserDTO
    {
        public string Name { get; set; } = string.Empty;
        public Guid Id_user_level { get; set; } 
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public Boolean Is_active { get; set; }


    }
}