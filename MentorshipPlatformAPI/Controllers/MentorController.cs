using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MentorshipPlatformAPI.Models;
using System.Linq;
using System.Threading.Tasks;
using MentorshipPlatformAPI.Data;

namespace MentorshipPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MentorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Get all mentors
        [HttpGet("getmentors")]
        public async Task<IActionResult> GetMentors()
        {
            var mentors = await _context.Profiles
                .Where(p => p.Role.ToLower() == "mentor")
                .ToListAsync(); // Get all mentors first
            return Ok(mentors);
        }

        // Match mentors based on user skills
        [HttpPost("matchmentors")]
        public async Task<IActionResult> MatchMentors([FromBody] string userSkills)
        {
            var userSkillsArray = userSkills.ToLower().Split(',').Select(skill => skill.Trim()).ToArray();

            // Get all mentors with Role "Mentor"
            var mentors = await _context.Profiles
                .Where(mentor => mentor.Role == "Mentor")
                .ToListAsync();

            // Filter mentors in-memory based on skills
            var matchedMentors = mentors
                .Where(mentor => mentor.Skills != null &&
                                 mentor.Skills.ToLower()
                                 .Split(',')
                                 .Select(skill => skill.Trim())
                                 .Any(mentorSkill => userSkillsArray.Contains(mentorSkill)))
                .ToList();

            return Ok(matchedMentors);
        }
    }
}
