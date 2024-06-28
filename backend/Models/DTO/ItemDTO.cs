using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models.DTO
{
    public class ItemDTO
    {
        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(1000)]
        public string Image { get; set; }

        [Required]
        public int ServesCount { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public int CuisineId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ItemDTO()
        {
            Name = "";
            Image = "";
            ServesCount = 0;
            Price = 0M;
            CuisineId = 0;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}