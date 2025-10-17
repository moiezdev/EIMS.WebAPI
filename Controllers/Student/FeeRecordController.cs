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
    public class FeeRecordController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FeeRecordController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FeeRecord
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<FeeRecord>>> GetFeeRecords()
        {
            return await _context.FeeRecords.ToListAsync();
        }

        // GET: api/FeeRecord/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Educator,Student")]
        public async Task<ActionResult<FeeRecord>> GetFeeRecord(int id)
        {
            var record = await _context.FeeRecords.FindAsync(id);
            if (record == null)
            {
                return NotFound(new { Message = "Fee payment record not found" });
            }

            // Get the current user's role and UserId from the token
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Admin can access any fee payment record
            if (currentUserRole == "Admin")
            {
                return Ok(record);
            }

            // Educator can access fee payment records for students
            if (currentUserRole == "Educator")
            {
                var studentRecord = await _context.Users.FindAsync(record.UserId);
                if (studentRecord != null && studentRecord.Role == "Student")
                {
                    return Ok(record);
                }
            }

            // Student can only access their own fee payment record
            if (currentUserRole == "Student" && currentUserId == record.UserId)
            {
                return Ok(record);
            }

            return Forbid("You are not authorized to access this fee payment record.");
        }

        // POST: api/FeeRecord
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<FeeRecord>> CreateFeeRecord(FeeRecord record)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the UserId exists in the Users table
            var user = await _context.Users.FindAsync(record.UserId);
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid UserId. User does not exist." });
            }

            _context.FeeRecords.Add(record);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFeeRecord), new { id = record.PaymentId }, record);
        }

        // PUT: api/FeeRecord/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateFeeRecord(int id, FeeRecord updatedRecord)
        {
            if (id != updatedRecord.PaymentId)
            {
                return BadRequest(new { Message = "Payment ID mismatch" });
            }

            var record = await _context.FeeRecords.FindAsync(id);
            if (record == null)
            {
                return NotFound(new { Message = "Fee payment record not found" });
            }

            // Ensure the UserId exists in the Users table
            var user = await _context.Users.FindAsync(updatedRecord.UserId);
            if (user == null)
            {
                return BadRequest(new { Message = "Invalid UserId. User does not exist." });
            }

            // Admin or Educator can update fee payment records
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (currentUserRole == "Admin" || currentUserRole == "Educator")
            {
                record.UserId = updatedRecord.UserId;
                record.AmountPaid = updatedRecord.AmountPaid;
                record.TotalFee = updatedRecord.TotalFee;
                record.PaymentDate = updatedRecord.PaymentDate;
                record.PaymentMethod = updatedRecord.PaymentMethod;
                record.Remarks = updatedRecord.Remarks;

                await _context.SaveChangesAsync();
                return NoContent();
            }

            return Forbid("You are not authorized to update this fee payment record.");
        }

        // DELETE: api/FeeRecord/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteFeeRecord(int id)
        {
            var record = await _context.FeeRecords.FindAsync(id);
            if (record == null)
            {
                return NotFound(new { Message = "Fee payment record not found" });
            }

            _context.FeeRecords.Remove(record);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}