using System;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class UserProfileAddress
    {
        public Guid Id { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public Guid UserProfileId { get; set; }

    }
}
