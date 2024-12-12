using MentorshipPlatformAPI.Data;
using Microsoft.AspNetCore.Mvc;

namespace MentorshipPlatformAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeleteController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DeleteController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DELETE: api/delete/delete-account/{id}
        [HttpDelete("delete-account/{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            // Try to find the user in the Registers table first
            var register = await _context.Registers.FindAsync(id);
            if (register == null)
            {
                return NotFound(new { message = "User not found in Register table" });
            }

            try
            {
                // Remove from Registers table
                _context.Registers.Remove(register);
                await _context.SaveChangesAsync();

                // After deleting from Register, check and delete from Profiles table
                var profile = await _context.Profiles.FindAsync(id);
                if (profile != null)
                {
                    _context.Profiles.Remove(profile);
                    await _context.SaveChangesAsync();
                }

                return Ok(new { message = "User deleted from Register and Profile tables" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the account", details = ex.Message });
            }
        }
    }
}
