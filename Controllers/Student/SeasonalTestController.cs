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
    public class SeasonalTestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SeasonalTestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SeasonalTest
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<SeasonalTest>>> GetSeasonalTests()
        {
            return await _context.SeasonalTests.ToListAsync();
        }

        // GET: api/SeasonalTest/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Educator,Student")]
        public async Task<ActionResult<SeasonalTest>> GetSeasonalTest(int id)
        {
            var test = await _context.SeasonalTests.FindAsync(id);
            if (test == null)
            {
                return NotFound(new { Message = "Test not found" });
            }

            // Get the current user's role and UserId from the token
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            // Admin can access any test
            if (currentUserRole == "Admin")
            {
                return Ok(test);
            }

            // Educator can access tests for students
            if (currentUserRole == "Educator")
            {
                var studentTest = await _context.Users.FindAsync(test.UserId);
                if (studentTest != null && studentTest.Role == "Student")
                {
                    return Ok(test);
                }
            }

            // Student can only access their own test
            if (currentUserRole == "Student" && currentUserId == test.UserId)
            {
                return Ok(test);
            }

            return Forbid("You are not authorized to access this test.");
        }

        // POST: api/SeasonalTest
        [HttpPost]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<ActionResult<SeasonalTest>> CreateSeasonalTest(SeasonalTest test)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.SeasonalTests.Add(test);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSeasonalTest), new { id = test.TestId }, test);
        }

        // PUT: api/SeasonalTest/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Educator")]
        public async Task<IActionResult> UpdateSeasonalTest(int id, SeasonalTest updatedTest)
        {
            if (id != updatedTest.TestId)
            {
                return BadRequest(new { Message = "Test ID mismatch" });
            }

            var test = await _context.SeasonalTests.FindAsync(id);
            if (test == null)
            {
                return NotFound(new { Message = "Test not found" });
            }

            // Admin can update any test
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (currentUserRole == "Admin" || currentUserRole == "Educator")
            {
                // Update fields
                test.UserId = updatedTest.UserId;
                test.English = updatedTest.English;
                test.EnglishTotal = updatedTest.EnglishTotal;
                test.Urdu = updatedTest.Urdu;
                test.UrduTotal = updatedTest.UrduTotal;
                test.Islamiat = updatedTest.Islamiat;
                test.IslamiatTotal = updatedTest.IslamiatTotal;
                test.Arabic = updatedTest.Arabic;
                test.ArabicTotal = updatedTest.ArabicTotal;
                test.GKnowlege = updatedTest.GKnowlege;
                test.GKnowlegeTotal = updatedTest.GKnowlegeTotal;
                test.Math = updatedTest.Math;
                test.MathTotal = updatedTest.MathTotal;
                test.Chemistry = updatedTest.Chemistry;
                test.ChemistryTotal = updatedTest.ChemistryTotal;
                test.Physics = updatedTest.Physics;
                test.PhysicsTotal = updatedTest.PhysicsTotal;
                test.Biology = updatedTest.Biology;
                test.SocialStudies = updatedTest.SocialStudies;
                test.GainedMarks = updatedTest.GainedMarks;
                test.TotalMarks = updatedTest.TotalMarks;
                test.Date = updatedTest.Date;
                test.Season = updatedTest.Season;
                test.Year = updatedTest.Year;

                await _context.SaveChangesAsync();
                return NoContent();
            }

            return Forbid("You are not authorized to update this test.");
        }

        // DELETE: api/SeasonalTest/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSeasonalTest(int id)
        {
            var test = await _context.SeasonalTests.FindAsync(id);
            if (test == null)
            {
                return NotFound(new { Message = "Test not found" });
            }

            _context.SeasonalTests.Remove(test);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}