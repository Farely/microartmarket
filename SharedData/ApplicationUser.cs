using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SharedData
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public ICollection<UserArtTag> Tags { get; set; }

        public string DescriptionText { get; set; }

        public string ShortStatus { get; set; }
    }
}