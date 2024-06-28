using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(Email), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }

        public enum UserType
        {
            Customer = 0, Caterer = 1, Admin = 2
        }

        [Required]
        [Range(0, 2)]
        public UserType Type { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 4)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public User()
        {
            Id = 0;
            Type = 0;
            Email = "";
            Password = "";
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
