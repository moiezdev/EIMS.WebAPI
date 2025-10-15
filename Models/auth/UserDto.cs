using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace EIMS.WebAPI.Models
{
    public class UserDto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the ID
        public string? UserId { get; set; } // Nullable to make it optional

        [Required]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [MaxLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [MaxLength(20, ErrorMessage = "Government ID cannot exceed 20 characters.")]
        public string GovernmentId { get; set; } = string.Empty;

        [Required]
        [MaxLength(50, ErrorMessage = "Guardian name cannot exceed 50 characters.")]
        public string GuardianName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Gender cannot exceed 10 characters.")]
        public string Gender { get; set; } = string.Empty;

        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(20, ErrorMessage = "Role cannot exceed 20 characters.")]
        public string Role { get; set; } = string.Empty; // e.g., Student, Educator, Admin

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone(ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(250, ErrorMessage = "Address cannot exceed 250 characters.")]
        public string? Address { get; set; }
    }
}