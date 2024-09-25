using IdentitySample.Models.DTOs;
using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySample.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterDTO dto)
        {
            if(!ModelState.IsValid)
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
            if(result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            string message = "";
            foreach(var item in result.Errors)
            {
                message += item.Description + Environment.NewLine;
            }
            TempData["Message"] = message;

            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            LoginDTO dto = new LoginDTO
            {
                ReturnUrl = returnUrl
            };

            return View(dto);
        }

        [HttpPost]
        public IActionResult Login(LoginDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = _userManager.FindByNameAsync(dto.UserName).Result;

            _signInManager.SignOutAsync();
            var result = _signInManager.PasswordSignInAsync(user, dto.Password, dto.IsPersistent, true).Result;

            if(result.Succeeded)
            {
                return Redirect(dto.ReturnUrl);
            }

            if(result.RequiresTwoFactor)
            {
                //go to page for TwoFactor
            }

            if(result.IsLockedOut)
            {
                // go to LockedOut
            }

            ModelState.AddModelError(string.Empty, "login error");

            return View();
        }

        [HttpGet]
        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
