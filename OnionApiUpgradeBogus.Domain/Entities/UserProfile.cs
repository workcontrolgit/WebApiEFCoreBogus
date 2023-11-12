using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionApiUpgradeBogus.Domain.Entities
{
    public class UserProfile
    {
        public Guid Id { get; set; }
        public UserInfo User { get; set; }
        public ICollection<UserProfileAddress> Addresses { get; set; }
    }
}
