using MentorshipPlatformAPI.Data;
using MentorshipPlatformAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace MentorshipPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;

        public AuthController(IConfiguration config, ApplicationDbContext context)
        {
            _config = config;
            _context = context;
        }

        // POST: /api/Auth/register
        [HttpPost("register")]
        public IActionResult Register([FromBody] Register register)
        {
            // Check if email already exists
            if (_context.Registers.Any(u => u.Email == register.Email))
            {
                return Conflict("Email already exists.");
            }

            // Create new user object
            var newUser = new Register
            {
                Username = register.Username,
                Role = register.Role, // Role should come from the frontend (mentor/mentee)
                Email = register.Email,
                Password = register.Password // Ideally, password should be hashed
            };

            // Add to the database
            _context.Registers.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered successfully.");
        }

        // POST: /api/Auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] Login login)
        {
            var user = AuthenticateUser(login);

            if (user != null)
            {
                // Generate token
                var tokenString = GenerateJwtToken(user);

                // Return token, username, role, and id in response
                return Ok(new
                {
                    token = tokenString,
                    username = user.Username,
                    role = user.Role,
                    id = user.Id
                });
            }

            return Unauthorized("Invalid email or password.");
        }

        private Register AuthenticateUser(Login login)
        {
            // Validate user from the database
            return _context.Registers.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
        }

        private string GenerateJwtToken(Register user)
        {
            // Generate JWT token
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim("username", user.Username),
                new Claim("role", user.Role),  // Add the role claim
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", user.Id.ToString())  // Add the user ID claim
            };

            // Create the JWT security token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
                signingCredentials: creds);

            // Return the token as a string
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
