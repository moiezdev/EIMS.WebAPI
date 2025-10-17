using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIMS.WebAPI.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProjectId { get; set; } // Auto-generated Project ID

        [Required]
        public string UserId { get; set; } = string.Empty; // Foreign key to User

        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty; // Project title

        [Required]
        [MaxLength(500)]
        public string Description { get; set; } = string.Empty; // Project description

        [Required]
        public DateTime SubmissionDate { get; set; } // Submission date

        [Required]
        [MaxLength(100)]
        public string Supervisor { get; set; } = string.Empty; // Supervisor name

        [Required]
        [MaxLength(100)]
        public string Remarks { get; set; } = string.Empty; // Supervisor email

        [Required]
        public string SupervisorId { get; set; } = string.Empty; // Foreign key to Supervisor's UserId
    }
}