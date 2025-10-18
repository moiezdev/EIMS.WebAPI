using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIMS.WebAPI.Models
{
    public class StudentClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassId { get; set; } // Auto-generated Class ID

        [Required]
        public string UserId { get; set; } = string.Empty; // Foreign key to User

        [Required]
        [MaxLength(50)]
        public string Class { get; set; } = string.Empty; // Class name (e.g., 10th Grade)

        [Required]
        [MaxLength(250)]
        public string Subjects { get; set; } = string.Empty; // List of subjects

        [Required]
        public int Year { get; set; } // Academic year

        [Required]
        [MaxLength(100)]
        public string ClassIncharge { get; set; } = string.Empty; // Name of the class incharge

        [Required]
        public string InchargeId { get; set; } = string.Empty; // Foreign key to the incharge's UserId
    }
}