namespace MentorshipPlatformAPI.Models
{
    public class MentorshipRequest
    {
        public int? Id { get; set; }
        public int MentorId { get; set; }
        public int MenteeId { get; set; }
        public string Status { get; set; } // "Pending", "Accepted", "Rejected"
    }
}
