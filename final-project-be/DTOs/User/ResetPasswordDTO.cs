namespace final_project_be.DTOs.User
{
    public class ResetPasswordDTO
    {
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

    }
}
