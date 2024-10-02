using Microsoft.AspNetCore.Mvc.Rendering;

namespace IdentitySample.Areas.Admin.Models.DTOs.UserRole
{
    public class UserRoleCreateDTO
    {
        public string Id { get; set; }

        public string Role { get; set; }

        public List<SelectListItem> Roles { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }
    }
}
