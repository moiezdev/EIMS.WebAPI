using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIMS.WebAPI.Models
{
    public class SeasonalTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TestId { get; set; } // Auto-generated Test ID

        [Required]
        public string UserId { get; set; } = string.Empty; // Foreign key to User

        public int English { get; set; }
        public int EnglishTotal { get; set; }
        public int Urdu { get; set; }
        public int UrduTotal { get; set; }
        public int Islamiat { get; set; }
        public int IslamiatTotal { get; set; }
        public int Arabic { get; set; }
        public int ArabicTotal { get; set; }
        public int GKnowlege { get; set; }
        public int GKnowlegeTotal { get; set; }
        public int Math { get; set; }
        public int MathTotal { get; set; }
        public int Chemistry { get; set; }
        public int ChemistryTotal { get; set; }
        public int Physics { get; set; }
        public int PhysicsTotal { get; set; }
        public int Biology { get; set; }
        public int BiologyTotal { get; set; }
        public int SocialStudies { get; set; }
        public int SocialStudiesTotal { get; set; }
        public int Computer { get; set; }
        public int ComputerTotal { get; set; }
        public int Economics { get; set; }
        public int EconomicsTotal { get; set; }
        public int GainedMarks { get; set; }
        public int TotalMarks { get; set; }
        public double Percentage { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(50)]
        public string Season { get; set; } = string.Empty; // e.g., Spring, Fall

        [Required]
        public int Year { get; set; }
    }
}