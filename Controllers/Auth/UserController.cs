using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using EIMS.WebAPI.Data;
using EIMS.WebAPI.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace EIMS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Educator,Student")]
        public ActionResult<User> GetUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (currentUserRole == "Admin" && (user.Role == "Student" || user.Role == "Educator"))
            {
                return Ok(user);
            }

            if (currentUserRole == "Educator" && user.Role == "Student")
            {
                return Ok(user);
            }

            if (currentUserRole == "Student" && currentUserId == id)
            {
                return Ok(user);
            }

            return Forbid();
        }

        // GET: api/User
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // POST: api/User/register
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public ActionResult<User> Register(UserDto newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string rolePrefix = newUser.Role switch
            {
                "Student" => "ST",
                "Educator" => "ED",
                "Admin" => "AD",
                _ => throw new ArgumentException("Invalid role specified")
            };

            var lastUser = _context.Users
                .Where(u => u.UserId != null && u.UserId.StartsWith($"OXF_{rolePrefix}_"))
                .OrderByDescending(u => u.UserId)
                .FirstOrDefault();

            newUser.UserId = lastUser == null
                ? $"OXF_{rolePrefix}_001"
                : $"OXF_{rolePrefix}_{(int.Parse(lastUser?.UserId?.Split('_').Last() ?? "0") + 1):D3}";

            var passwordHasher = new PasswordHasher<User>();
            var user = new User
            {
                UserId = newUser.UserId,
                FirstName = newUser.FirstName,
                LastName = newUser.LastName,
                GovernmentId = newUser.GovernmentId,
                GuardianName = newUser.GuardianName,
                DateOfBirth = newUser.DateOfBirth.ToUniversalTime(),
                Gender = newUser.Gender,
                Role = newUser.Role,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Address = newUser.Address,
                RegistrationDate = DateTime.UtcNow,
                PasswordHash = passwordHasher.HashPassword(null, newUser.Password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, user);
        }

        // POST: api/User/login
        [HttpPost("login")]
        public ActionResult Login(UserLoginDto loginDto)
        {
            var user = _context.Users.SingleOrDefault(u => u.UserId == loginDto.UserId);
            if (user == null)
            {
                return Unauthorized(new { Message = "Invalid UserId or Password" });
            }

            var passwordHasher = new PasswordHasher<User>();
            var result = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { Message = "Invalid Password" });
            }

            string token = CreateToken(user);
            return Ok(new { Token = token });
        }

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult UpdateUser(string id, [FromBody] User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (currentUserRole == "Admin" || (currentUserRole == "Educator" && user.Role == "Student") || (currentUserRole == "Student" && currentUserId == id))
            {
                UpdateUserDetails(user, updatedUser);
                _context.SaveChanges();
                return NoContent();
            }
            return Forbid();
        }

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteUser(string id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }

        // Helper method to update user details
        private void UpdateUserDetails(User user, User updatedUser)
        {
            if (!string.IsNullOrWhiteSpace(updatedUser.FirstName)) user.FirstName = updatedUser.FirstName;
            if (!string.IsNullOrWhiteSpace(updatedUser.LastName)) user.LastName = updatedUser.LastName;
            if (!string.IsNullOrWhiteSpace(updatedUser.GovernmentId)) user.GovernmentId = updatedUser.GovernmentId;
            if (!string.IsNullOrWhiteSpace(updatedUser.GuardianName)) user.GuardianName = updatedUser.GuardianName;
            if (updatedUser.DateOfBirth != default) user.DateOfBirth = updatedUser.DateOfBirth.ToUniversalTime();
            if (!string.IsNullOrWhiteSpace(updatedUser.Gender)) user.Gender = updatedUser.Gender;
            if (!string.IsNullOrWhiteSpace(updatedUser.Email)) user.Email = updatedUser.Email;
            if (!string.IsNullOrWhiteSpace(updatedUser.PhoneNumber)) user.PhoneNumber = updatedUser.PhoneNumber;
            if (!string.IsNullOrWhiteSpace(updatedUser.Address)) user.Address = updatedUser.Address;

            if (User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value == "Admin" && !string.IsNullOrWhiteSpace(updatedUser.Role))
            {
                user.Role = updatedUser.Role;
            }
        }

        private string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId ?? string.Empty),
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var jwtKey = _configuration.GetValue<string>("Jwt:Key") ?? throw new InvalidOperationException("JWT key is not configured.");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "EIMS.WebAPI",
                audience: "EIMS.WebAPI",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}