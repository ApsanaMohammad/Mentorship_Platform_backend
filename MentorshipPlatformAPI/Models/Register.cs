namespace MentorshipPlatformAPI.Models
{
    public class Register
    {
        public int? Id { get; set; }  // Primary Key
        public string? Username { get; set; }
        public string? Role { get; set; } // mentee or mentor
        public string? Email { get; set; }
        public string? Password { get; set; }

        
    }
}
