using System;
namespace AlStudente.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string FirebaseUserId { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string ImageLocation { get; set; }
        public int UserTypeId { get; set; }
        public int InstrumentId { get; set; }
        public string Bio { get; set }
        public DateTime CreateDateTime { get; set; }
    }
}
