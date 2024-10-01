using IdentitySample.Areas.Admin.Models.DTOs;
using IdentitySample.Areas.Admin.Models.DTOs.Role;
using IdentitySample.Models.DTOs;
using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySample.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles
                            .Select(p => new RoleListDTO
                            {
                                Id = p.Id,
                                Name = p.Name,
                                Description = p.Description
                            }).ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(RoleCreateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            Role role = new Role()
            {
                Name = dto.Name,
                Description = dto.Description
            };

            var result = _roleManager.CreateAsync(role).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role", new { area = "Admin" });
            }

            ViewBag.Errors = result.Errors.ToList();

            return View();
        }

        public IActionResult Edit(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;
            RoleEditDTO roleEditDTO = new RoleEditDTO()
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };

            return View(roleEditDTO);
        }

        [HttpPost]
        public IActionResult Edit(RoleEditDTO dto)
        {
            var role = _roleManager.FindByIdAsync(dto.Id).Result;

            role.Name = dto.Name;
            role.Description = dto.Description;

            var result = _roleManager.UpdateAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role", new { area = "Admin" });
            }

            ViewBag.Errors = result.Errors.ToList();

            return View(dto);
        }

        public IActionResult Delete(string id)
        {
            var role = _roleManager.FindByIdAsync(id).Result;

            RoleDeleteDTO dto = new RoleDeleteDTO()
            {
                Id = role.Id,
                Name = role.Name
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult Delete(RoleDeleteDTO dto)
        {
            var role = _roleManager.FindByIdAsync(dto.Id).Result;

            var result = _roleManager.DeleteAsync(role).Result;

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Role", new { area = "Admin" });
            }

            ViewBag.Errors = result.Errors.ToList();

            return View(dto);
        }
    }
}
