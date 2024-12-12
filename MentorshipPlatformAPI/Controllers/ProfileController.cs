using Microsoft.AspNetCore.Mvc;
using MentorshipPlatformAPI.Data;
using MentorshipPlatformAPI.Models;

namespace MentorshipPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProfileController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("saveprofile")]
        public async Task<IActionResult> UpdateProfile([FromBody] Profile profile)
        {
            if (profile == null)
            {
                return BadRequest("Profile data is null");
            }

            var existingProfile = await _context.Profiles.FindAsync(profile.Id);

            if (existingProfile != null)
            {
                
                existingProfile.Id = profile.Id;
                existingProfile.Username = profile.Username;
                existingProfile.Role = profile.Role;
                existingProfile.Phone = profile.Phone;
                existingProfile.Bio = profile.Bio;
                existingProfile.Skills = profile.Skills;
                existingProfile.Location = profile.Location;
            }
            else
            {
                // Add new profile
                await _context.Profiles.AddAsync(profile);
            }

            await _context.SaveChangesAsync();
            return Ok("Profile updated successfully");
        }
    }
}

