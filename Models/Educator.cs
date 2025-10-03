using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace EIMS.WebAPI.Models
{
    public class Educator
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the ID
        public string? EducatorId { get; set; } // Nullable to make it optional
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string GovernmentId { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public required DateTime DateJoined { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
        public required string Qualification { get; set; }
        public string? Specialization { get; set; }
    }
}