﻿namespace final_project_be.DTOs.Category
{
    public class CategoryDTO
    {

        public string Name { get; set; } = string.Empty;

        public string? Img { get; set; }
        public IFormFile? ImageFile { get; set; }
        public string Description { get; set; } = string.Empty;
        public Boolean Is_active { get; set; }
    }
}
