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
    public class ProjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Project
        [HttpGet]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            return await _context.Projects.ToListAsync();
        }

        // GET: api/Project/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Educator,Student")]
        public async Task<ActionResult<Project>> GetProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound(new { Message = "Project not found" });
            }

            // Get the current user's role and UserId from the token
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Admin and Educator can access any project
            if (currentUserRole == "Admin" || currentUserRole == "Educator")
            {
                return Ok(project);
            }

            // Student can only access their own project
            if (currentUserRole == "Student" && currentUserId == project.UserId)
            {
                return Ok(project);
            }

            return Forbid("You are not authorized to access this project.");
        }

        // POST: api/Project
        [HttpPost]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<ActionResult<Project>> CreateProject(Project project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the UserId exists in the Users table
            var user = await _context.Users.FindAsync(project.UserId);
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid UserId. User does not exist." });
            }

            _context.Projects.Add(project);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProject), new { id = project.ProjectId }, project);
        }

        // PUT: api/Project/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<IActionResult> UpdateProject(int id, Project updatedProject)
        {
            if (id != updatedProject.ProjectId)
            {
                return BadRequest(new { Message = "Project ID mismatch" });
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound(new { Message = "Project not found" });
            }

            // Ensure the UserId exists in the Users table
            var user = await _context.Users.FindAsync(updatedProject.UserId);
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid UserId. User does not exist." });
            }

            // Update fields
            project.UserId = updatedProject.UserId;
            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;
            project.SubmissionDate = updatedProject.SubmissionDate;
            project.Supervisor = updatedProject.Supervisor;
            project.SupervisorId = updatedProject.SupervisorId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Project/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProject(int id)
        {
            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound(new { Message = "Project not found" });
            }

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}