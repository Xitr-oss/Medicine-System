using System;

namespace backend.DTOs
{
    public class MedicineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public bool? IsActive { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class CreateMedicineDto
    {
        public string Name { get; set; } = null!;
        public int CategoryId { get; set; }
        public decimal Price { get; set; }
        public string? Description { get; set; }
        public int Stock { get; set; }
        public bool? IsActive { get; set; }
        public string? ImageUrl { get; set; }
    }
}
