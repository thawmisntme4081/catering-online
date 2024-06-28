using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public User? Customer { get; set; }

        [Required]
        public int CatererId { get; set; }
        [ForeignKey(nameof(CatererId))]
        public Caterer? Caterer { get; set; }

        [Required]
        public DateOnly BookingDate { get; set; }

        [Required]
        public DateOnly EventDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Venue { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public Booking()
        {
            Id = 0;
            CustomerId = 0;
            Customer = null;
            CatererId = 0;
            Caterer = null;
            BookingDate = new DateOnly();
            EventDate = new DateOnly();
            Venue = "";
            TotalAmount = 0;
            Status = "Pending";
            PaymentMethod = "";
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
