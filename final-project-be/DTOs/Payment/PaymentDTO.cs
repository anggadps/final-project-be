namespace final_project_be.DTOs.Payment
{
    public class PaymentDTO
    {
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
        public IFormFile? ImageFile { get; set; }

        public Boolean Is_active { get; set; }

    }
}
