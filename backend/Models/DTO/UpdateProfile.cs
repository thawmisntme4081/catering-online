using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTO
{
    public class UpdateProfile
    {
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

        public string Image { get; set; }

        public UpdateProfile()
        {
            FirstName = "";
            LastName = "";
            PhoneNumber = "";
            Address = "";
            Image = "";
        }
    }
}
