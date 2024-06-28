using System.ComponentModel.DataAnnotations;

namespace backend.Models.DTO
{
    public class Login
    {
        [Required]
        [StringLength(255, MinimumLength = 4)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }

        public Login()
        {
            Email = "";
            Password = "";
        }
    }
}
