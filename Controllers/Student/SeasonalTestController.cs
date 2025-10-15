using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EIMS.WebAPI.Data;
using EIMS.WebAPI.Models;

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
        public async Task<ActionResult<IEnumerable<SeasonalTest>>> GetSeasonalTests()
        {
            return await _context.SeasonalTests.ToListAsync();
        }

        // GET: api/SeasonalTest/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<SeasonalTest>> GetSeasonalTest(int id)
        {
            var test = await _context.SeasonalTests.FindAsync(id);
            if (test == null)
            {
                return NotFound(new { Message = "Test not found" });
            }
            return Ok(test);
        }

        // POST: api/SeasonalTest
        [HttpPost]
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

        // DELETE: api/SeasonalTest/{id}
        [HttpDelete("{id}")]
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