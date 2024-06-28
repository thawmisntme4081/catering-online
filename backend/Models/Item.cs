using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Item
    {
        [Key]
        public int Id { get; set; }

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
        public int CatererId { get; set; }
        [ForeignKey(nameof(CatererId))]
        public Caterer? Caterer { get; set; }

        [Required]
        public int CuisineId { get; set; }
        [ForeignKey(nameof(CuisineId))]
        public CuisineType? CuisineType { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Item()
        {
            Id = 0;
            Name = "";
            Image = "";
            ServesCount = 0;
            Price = 0M;
            CatererId = 0;
            Caterer = null;
            CuisineId = 0;
            CuisineType = null;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}