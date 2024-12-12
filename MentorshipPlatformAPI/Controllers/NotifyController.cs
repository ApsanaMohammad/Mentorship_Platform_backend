using Microsoft.AspNetCore.Mvc;
using MentorshipPlatformAPI.Models;
using MentorshipPlatformAPI.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MentorshipPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotifyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public NotifyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/notify/send
        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] MentorshipRequest request)
        {
            if (request == null || request.MentorId <= 0 || request.MenteeId <= 0)
            {
                return BadRequest(new { message = "Invalid mentor or mentee ID" });
            }

            var newRequest = new MentorshipRequest
            {
                MentorId = request.MentorId,
                MenteeId = request.MenteeId,
                Status = "Pending"
            };

            _context.MentorshipRequests.Add(newRequest);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Notification sent successfully", request = newRequest });
        }

        // PUT: api/notify/updatestatus/{id}
        [HttpPut("updatestatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var request = await _context.MentorshipRequests.FindAsync(id);
            if (request == null)
            {
                return NotFound(new { message = "Request not found" });
            }

            if (status != "Pending" && status != "Accepted" && status != "Rejected")
            {
                return BadRequest(new { message = "Invalid status" });
            }

            request.Status = status;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Request status updated", request });
        }

        // GET: api/notify/mentee/{menteeId}
        [HttpGet("mentee/{menteeId}")]
        public async Task<IActionResult> GetNotificationsForMentee(int menteeId)
        {
            try
            {
                // Fetching the mentorship requests for the menteeId
                var notifications = await _context.MentorshipRequests
                    .Where(r => r.MenteeId == menteeId)
                    .Select(r => new
                    {
                        r.Id,
                        r.MentorId,
                        r.Status
                    })
                    .ToListAsync();

                if (notifications == null || notifications.Count == 0)
                {
                    return NotFound(new { message = "No notifications found for this mentee." });
                }

                return Ok(notifications);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching notifications.", error = ex.Message });
            }
        }


        [HttpGet("mentor/{mentorId}")]
        public async Task<IActionResult> GetNotificationsForMentor(int mentorId)
        {
            try
            {
                // Fetching the mentorship requests for the mentorId
                var notifications = await _context.MentorshipRequests
                    .Where(r => r.MentorId == mentorId)
                    .Select(r => new
                    {
                        r.Id,
                        r.MenteeId,
                        r.Status
                    })
                    .ToListAsync();

                if (notifications == null || notifications.Count == 0)
                {
                    return NotFound(new { message = "No requests found for this mentor." });
                }

                return Ok(notifications);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching notifications.", error = ex.Message });
            }
        }
    }
}
