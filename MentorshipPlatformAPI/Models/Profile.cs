namespace MentorshipPlatformAPI.Models
{
    public class Profile
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string Phone { get; set; }
        public string Bio { get; set; }
        public string? Skills { get; set; }
        public string Location { get; set; }
    }
}
