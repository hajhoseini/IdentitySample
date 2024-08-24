using Microsoft.AspNetCore.Identity;

namespace IdentitySample.Models.Entities
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}
