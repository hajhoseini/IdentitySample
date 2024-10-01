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

        public IActionResult Edit(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            UserEditDTO userEditDTO = new UserEditDTO()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserName = user.UserName,
            };

            return View(userEditDTO);
        }

        [HttpPost]
        public IActionResult Edit(UserEditDTO dto)
        {
            var user = _userManager.FindByIdAsync(dto.Id).Result;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            user.PhoneNumber = dto.PhoneNumber;
            user.Email = dto.Email;
            user.UserName = dto.UserName;

            var result = _userManager.UpdateAsync(user).Result;

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

            return View(dto);
        }

        public IActionResult Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            UserDeleteDTO dto = new UserDeleteDTO()
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                UserName = user.UserName,
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult Delete(UserDeleteDTO dto)
        {
            var user = _userManager.FindByIdAsync(dto.Id).Result;

            var result = _userManager.DeleteAsync(user).Result;

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

            return View(dto);
        }
    }
}
