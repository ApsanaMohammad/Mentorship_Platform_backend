using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MentorshipPlatformAPI.Models; // Ensure this matches your namespace
using System.Linq;
using System.Threading.Tasks;
using MentorshipPlatformAPI.Data;

namespace MentorshipPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MentorshipRequestController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MentorshipRequestController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Fetch mentee requests for a specific mentor
        [HttpGet("mentor/{mentorId}")]
        public async Task<IActionResult> GetMenteeRequests(int mentorId)
        {
            try
            {
                var requests = await _context.MentorshipRequests
                    .Where(r => r.MentorId == mentorId)
                    .OrderByDescending(r => r.Id)
                    .ToListAsync();

                var latestRequests = requests
                    .GroupBy(r => r.MenteeId)
                    .Select(group => group.First())
                    .Select(r => new
                    {
                        r.Id,
                        r.MenteeId,
                        r.Status
                    })
                    .ToList();

                if (latestRequests == null || latestRequests.Count == 0)
                {
                    return NotFound(new { message = "No mentee requests found for this mentor." });
                }

                return Ok(latestRequests);
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while fetching mentee requests.", error = ex.Message });
            }
        }

        // Accept or reject a mentee request
        [HttpPut("mentor/accept-reject/{requestId}")]
        public async Task<IActionResult> UpdateRequestStatus(int requestId, [FromBody] ActionRequest actionRequest)
        {
            try
            {
                // Find the request by requestId
                var request = await _context.MentorshipRequests.FindAsync(requestId);
                if (request == null)
                {
                    return NotFound(new { message = "Mentorship request not found." });
                }

                // Update the status based on the action (accept or reject)
                if (actionRequest.Action == "accept")
                {
                    request.Status = "Accepted";
                }
                else if (actionRequest.Action == "reject")
                {
                    request.Status = "Rejected";
                }
                else
                {
                    return BadRequest(new { message = "Invalid action. Must be 'accept' or 'reject'." });
                }

                // Save the changes to the database
                _context.MentorshipRequests.Update(request);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Request updated successfully." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the request.", error = ex.Message });
            }
        }
    }

    // Model to handle the action (accept or reject)
    public class ActionRequest
    {
        public string Action { get; set; }
    }
}
