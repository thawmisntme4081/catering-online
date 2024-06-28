using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int SenderId { get; set; }
        [ForeignKey(nameof(SenderId))]
        public User? Sender { get; set; }

        [Required]
        public int ReceiverId { get; set; }
        [ForeignKey(nameof(ReceiverId))]
        public User? Receiver { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Message()
        {
            Id = 0;
            SenderId = 0;
            Sender = null;
            ReceiverId = 0;
            Receiver = null;
            Content = "";
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
