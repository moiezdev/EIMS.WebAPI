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
    public class SportsRecordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SportsRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SportsRecord
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SportsRecord>>> GetSportsRecords()
        {
            return await _context.SportsRecords.ToListAsync();
        }

        // GET: api/SportsRecord/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Educator,Student")]
        public async Task<ActionResult<SportsRecord>> GetSportsRecord(int id)
        {
            var record = await _context.SportsRecords.FindAsync(id);
            if (record == null)
            {
                return NotFound(new { Message = "Sports record not found" });
            }

            // Get the current user's role and UserId from the token
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Admin can access any sports record
            if (currentUserRole == "Admin")
            {
                return Ok(record);
            }

            // Educator can access sports records for students
            if (currentUserRole == "Educator")
            {
                var studentRecord = await _context.Users.FindAsync(record.UserId);
                if (studentRecord != null && studentRecord.Role == "Student")
                {
                    return Ok(record);
                }
            }

            // Student can only access their own sports record
            if (currentUserRole == "Student" && currentUserId == record.UserId)
            {
                return Ok(record);
            }

            return Forbid("You are not authorized to access this sports record.");
        }

        // POST: api/SportsRecord
        [HttpPost]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<ActionResult<SportsRecord>> CreateSportsRecord(SportsRecord record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SportsRecords.Add(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSportsRecord), new { id = record.SportsId }, record);
        }

        // PUT: api/SportsRecord/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<IActionResult> UpdateSportsRecord(int id, SportsRecord updatedRecord)
        {
            if (id != updatedRecord.SportsId)
            {
                return BadRequest(new { Message = "Sports ID mismatch" });
            }

            var record = await _context.SportsRecords.FindAsync(id);
            if (record == null)
            {
                return NotFound(new { Message = "Sports record not found" });
            }

            // Admin or Educator can update sports records
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (currentUserRole == "Admin" || currentUserRole == "Educator")
            {
                record.UserId = updatedRecord.UserId;
                record.SportsId = updatedRecord.SportsId;
                record.SportName = updatedRecord.SportName;
                record.TeamId = updatedRecord.TeamId;
                record.TeamName = updatedRecord.TeamName;
                record.TeamRank = updatedRecord.TeamRank;
                record.PlayerRank = updatedRecord.PlayerRank;
                record.Date = updatedRecord.Date;

                await _context.SaveChangesAsync();
                return NoContent();
            }

            return Forbid("You are not authorized to update this sports record.");
        }

        // DELETE: api/SportsRecord/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSportsRecord(int id)
        {
            var record = await _context.SportsRecords.FindAsync(id);
            if (record == null)
            {
                return NotFound(new { Message = "Sports record not found" });
            }

            _context.SportsRecords.Remove(record);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}