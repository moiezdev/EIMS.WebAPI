using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EIMS.WebAPI.Data;
using EIMS.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace EIMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentClassController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentClassController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/StudentClass
        [HttpGet]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<ActionResult<IEnumerable<StudentClass>>> GetStudentClasses()
        {
            return await _context.StudentClasses.ToListAsync();
        }

        // GET: api/StudentClass/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Educator,Student")]
        public async Task<ActionResult<StudentClass>> GetStudentClass(int id)
        {
            var studentClass = await _context.StudentClasses.FindAsync(id);
            if (studentClass == null)
            {
                return NotFound(new { Message = "Student class not found" });
            }

            // Get the current user's role and UserId from the token
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Admin and Educator can access any student class
            if (currentUserRole == "Admin" || currentUserRole == "Educator")
            {
                return Ok(studentClass);
            }

            // Student can only access their own class
            if (currentUserRole == "Student" && currentUserId == studentClass.UserId)
            {
                return Ok(studentClass);
            }

            return Forbid("You are not authorized to access this student class.");
        }

        // POST: api/StudentClass
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<StudentClass>> CreateStudentClass(StudentClass studentClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the UserId exists in the Users table
            var user = await _context.Users.FindAsync(studentClass.UserId);
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid UserId. User does not exist." });
            }

            _context.StudentClasses.Add(studentClass);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStudentClass), new { id = studentClass.ClassId }, studentClass);
        }

        // PUT: api/StudentClass/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudentClass(int id, StudentClass updatedClass)
        {
            if (id != updatedClass.ClassId)
            {
                return BadRequest(new { Message = "Class ID mismatch" });
            }

            var studentClass = await _context.StudentClasses.FindAsync(id);
            if (studentClass == null)
            {
                return NotFound(new { Message = "Student class not found" });
            }

            // Ensure the UserId exists in the Users table
            var user = await _context.Users.FindAsync(updatedClass.UserId);
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid UserId. User does not exist." });
            }

            // Update fields
            studentClass.UserId = updatedClass.UserId;
            studentClass.Class = updatedClass.Class;
            studentClass.Subjects = updatedClass.Subjects;
            studentClass.Year = updatedClass.Year;
            studentClass.ClassIncharge = updatedClass.ClassIncharge;
            studentClass.InchargeId = updatedClass.InchargeId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/StudentClass/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudentClass(int id)
        {
            var studentClass = await _context.StudentClasses.FindAsync(id);
            if (studentClass == null)
            {
                return NotFound(new { Message = "Student class not found" });
            }

            _context.StudentClasses.Remove(studentClass);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}