using IdentitySample.Areas.Admin.Models.DTOs;
using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySample.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users
                            .Select(p => new UserListDTO
                            {
                                Id = p.Id,
                                FirstName = p.FirstName,
                                LastName = p.LastName,
                                UserName = p.UserName,
                                PhoneNumber = p.PhoneNumber,
                                EmailConfirmed = p.EmailConfirmed,
                                AccessFailedCount = p.AccessFailedCount
                            }).ToList();
            return View(users);
        }
    }
}
