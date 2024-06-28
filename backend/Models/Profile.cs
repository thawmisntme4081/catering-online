using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Profile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 2)]
        public string FirstName { get; set; }

        [StringLength(255, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 8)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Address { get; set; }

        [StringLength(1000)]
        public string Image { get; set; }

        public Profile()
        {
            Id = 0;
            User = null;
            UserId = 0;
            FirstName = "";
            LastName = "";
            PhoneNumber = "";
            Address = "";
            Image = "";
        }
    }
}
