using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Caterer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProfileId { get; set; }
        [ForeignKey(nameof(ProfileId))]
        public Profile? Profile { get; set; }

        public Caterer()
        {
            Id = 0;
            ProfileId = 0;
            Profile = null;
        }
    }
}