using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using EIMS.WebAPI.Data;
using EIMS.WebAPI.Models;

namespace EIMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EducatorController : ControllerBase
    {
        private readonly Data.ApplicationDbContext _context;

        public EducatorController(Data.ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Educator
        [HttpGet]
        public ActionResult<IEnumerable<Educator>> GetAllEducators()
        {
            return Ok(_context.Educators.ToList());
        }

        // GET: api/Educator/{id}
        [HttpGet("{id}")]
        public ActionResult<Educator> GetEducatorById(string id)
        {
            var Educator = _context.Educators.Find(id);
            if (Educator == null)
            {
                return NotFound(new { Message = "Educator not found" });
            }
            return Ok(Educator);
        }

        // POST: api/Educator
        [HttpPost]
        public ActionResult AddEducator([FromBody] Educator newEducator)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Generate a new EducatorId if not provided
            var lastEducator = _context.Educators.OrderByDescending(s => s.EducatorId).FirstOrDefault();
            if (lastEducator == null)
            {
                newEducator.EducatorId = "OXF_ED_001"; // Start with the first ID
            }
            else
            {
                // Extract the numeric part of the last EducatorId and increment it
                var lastIdNumber = int.Parse(lastEducator.EducatorId?.Split('_').Last() ?? "0");
                newEducator.EducatorId = $"OXF_ED_{(lastIdNumber + 1).ToString("D3")}";
            }

            _context.Educators.Add(newEducator);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetEducatorById), new { id = newEducator.EducatorId }, newEducator);
        }

        // PUT: api/Educator/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateEducator(string id, [FromBody] Educator updatedEducator)
        {
            var Educator = _context.Educators.Find(id);
            if (Educator == null)
            {
                return NotFound(new { Message = "Educator not found" });
            }

            // Update Educator details
            // Educator.Levels = updatedEducator.Levels;

            _context.SaveChanges();
            return NoContent();
        }

        // DELETE: api/Educator/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteEducator(string id)
        {
            var Educator = _context.Educators.Find(id);
            if (Educator == null)
            {
                return NotFound(new { Message = "Educator not found" });
            }

            _context.Educators.Remove(Educator);
            _context.SaveChanges();
            return NoContent();
        }
    }
}