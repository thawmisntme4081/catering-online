using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models {
    public class UserProfile {
        [Key]
        public int ID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        public int UserID { get; set; }
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
        public UserProfile() {
            ID = 0;
            User = null;
            UserID = 0;
            FirstName = "";
            LastName = "";
            PhoneNumber = "";
            Address = "";
            Image = "";
        }
    }
}
