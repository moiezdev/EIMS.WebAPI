using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using EIMS.WebAPI.Data;
using EIMS.WebAPI.Models;

namespace EIMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public StudentController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/student
        [HttpGet]
        public ActionResult<IEnumerable<Student>> GetAllStudents()
        {
            return Ok(_context.Students.ToList());
        }

        // GET: api/student/{id}
        [HttpGet("{id}")]
        public ActionResult<Student> GetStudentById(string id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound(new { Message = "Student not found" });
            }
            return Ok(student);
        }

        // POST: api/student
        [HttpPost]
        public ActionResult AddStudent([FromBody] Student newStudent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate a new StudentId if not provided
            var lastStudent = _context.Students.OrderByDescending(s => s.StudentId).FirstOrDefault();
            if (lastStudent == null)
            {
                newStudent.StudentId = "OXF_ST_001"; // Start with the first ID
            }
            else
            {
                // Extract the numeric part of the last StudentId and increment it
                var lastIdNumber = int.Parse(lastStudent.StudentId?.Split('_').Last() ?? "0");
                newStudent.StudentId = $"OXF_ST_{(lastIdNumber + 1).ToString("D3")}";
            }

            _context.Students.Add(newStudent);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetStudentById), new { id = newStudent.StudentId }, newStudent);
        }

        // PUT: api/student/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateStudent(string id, [FromBody] Student updatedStudent)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound(new { Message = "Student not found" });
            }

            // Update student details
            // student.Levels = updatedStudent.Levels;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/student/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteStudent(string id)
        {
            var student = _context.Students.Find(id);
            if (student == null)
            {
                return NotFound(new { Message = "Student not found" });
            }

            _context.Students.Remove(student);
            _context.SaveChanges();
            return NoContent();
        }
    }
}