using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIMS.WebAPI.Models
{
    public class SportsRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SportsId { get; set; } // Auto-generated Sports ID

        [Required]
        public string UserId { get; set; } = string.Empty; // Foreign key to User

        public string TeamId { get; set; } = string.Empty; // Foreign key to Team

        public string? TeamName { get; set; } // Foreign key to Player

        public int TeamRank { get; set; } // Total matches played

        [Required]
        [MaxLength(100)]
        public string SportName { get; set; } = string.Empty; // Name of the sport

        [Required]
        public int PlayerRank { get; set; } // Total matches won

        [Required]
        public DateTime Date { get; set; } // Date of the record
    }
}