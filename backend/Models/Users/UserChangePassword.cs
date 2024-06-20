using System.ComponentModel.DataAnnotations;

namespace backend.Models.Users {
    public class UserChangePassword {
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string NewPassword { get; set; }
        [Required]
        [StringLength(255, MinimumLength = 8)]
        public string OldPassword { get; set; }
        public UserChangePassword() {
            NewPassword = "";
            OldPassword = "";
        }
    }
}
