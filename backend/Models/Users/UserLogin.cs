using System.ComponentModel.DataAnnotations;

namespace backend.Models {
    public class UserLogin {
        [Required]
        [StringLength(255, MinimumLength = 4)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string Password { get; set; }
        public UserLogin() {
            Email = "";
            Password = "";
        }
    }
}
