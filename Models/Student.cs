using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace EIMS.WebAPI.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Auto-generate the ID
        public string? StudentId { get; set; } // Nullable to make it optional

        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public required string Gender { get; set; }
        public DateTime AdmissionDate { get; set; }
    }

    // public class Level
    // {
    //     public int Id { get; set; }  // <-- Primary Key
    //     public int LevelNumber { get; set; }
    //     public List<MonthlyGrade>? MonthlyGrades { get; set; }
    //     public List<Sport>? Sports { get; set; }
    //     public List<Project>? Projects { get; set; }

    //     [NotMapped] // Ignore this property in EF Core
    //     public List<BehaviourStatus>? BehaviourStatus { get; set; }

    //     public List<Complaint>? Complaints { get; set; }
    //     public RecordHistory? RecordHistory { get; set; }
    // }

    // public class Subject
    // {
    //     public int Id { get; set; }
    //     public string Name { get; set; }
    //     public int Grade { get; set; }
    //     public int MonthlyGradeId { get; set; }
    //     public MonthlyGrade MonthlyGrade { get; set; }
    // }

    // public class MonthlyGrade
    // {
    //     public int Id { get; set; } // Primary Key
    //     public string Month { get; set; }
    //     public List<Subject> Subjects { get; set; }
    //     public string Remarks { get; set; }
    // }

    // public class Sport
    // {
    //     public int Id { get; set; }  // Primary Key
    //     public string SportName { get; set; }
    //     public string Rank { get; set; }
    //     public DateTime Date { get; set; }
    //     public string Position { get; set; }
    // }

    // public class Project
    // {
    //     public int Id { get; set; }  // Primary Key
    //     public string Title { get; set; }
    //     public string Type { get; set; }
    //     public string Subject { get; set; }
    //     public DateTime SubmissionDate { get; set; }
    //     public string TeacherComments { get; set; }
    // }

    // public class BehaviourStatus
    // {
    //     public int Id { get; set; } // Add a primary key
    //     public DateTime Date { get; set; }
    //     public string Status { get; set; }
    //     public string TeacherComment { get; set; }
    // }

    // public class Complaint
    // {
    //     public int Id { get; set; }  // Primary Key
    //     public DateTime Date { get; set; }
    //     public string By { get; set; }
    //     public string ComplaintText { get; set; }
    // }

    // public class RecordHistory
    // {
    //     public int Id { get; set; }  // Primary Key
    //     public DateTime CreatedAt { get; set; }
    //     public DateTime LastUpdated { get; set; }
    // }
}