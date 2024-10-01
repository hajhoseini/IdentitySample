using IdentitySample.Areas.Admin.Models.DTOs;
using IdentitySample.Models.DTOs;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            User user = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                UserName = dto.Email
            };

            var result = _userManager.CreateAsync(user, dto.Password).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "User", new { area = "Admin" });
            }

            string message = "";
            foreach (var item in result.Errors)
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View();
        }
    }
}
