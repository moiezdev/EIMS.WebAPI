using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIMS.WebAPI.Models
{
    public class FeeRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; } // Auto-generated Payment ID

        [Required]
        public string UserId { get; set; } = string.Empty; // Foreign key to User

        [Required]
        public decimal AmountPaid { get; set; } // Amount paid by the user

        [Required]
        public decimal TotalFee { get; set; } // Total fee for the user

        [Required]
        public DateTime PaymentDate { get; set; } // Date of payment

        [Required]
        [MaxLength(50)]
        public string PaymentMethod { get; set; } = string.Empty; // e.g., Cash, Bank Transfer, etc.

        [MaxLength(250)]
        public string? Remarks { get; set; } // Optional remarks for the payment
    }
}