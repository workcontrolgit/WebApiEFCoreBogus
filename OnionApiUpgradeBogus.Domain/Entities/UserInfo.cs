using System;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public Guid UserProfileId { get; set; }
    }
}
