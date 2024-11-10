using IdentitySample.Models.DTOs;
using IdentitySample.Models.Entities;
using IdentitySample.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentitySample.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly EmailService _emailService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = new EmailService();
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
                //generate code ...
                var token = _userManager.GenerateEmailConfirmationTokenAsync(user).Result;
                string callBackUrl = Url.Action("ConfirmEmail", "Account", new { UserId = user.Id, Token = token }, protocol: Request.Scheme);
                string body = $"لطفا برای فعال سازی حساب کاربری بر روی لینک زیر کلیک کنید <br/> <a href = {callBackUrl}>Link</a>";
                _emailService.Execute(user.Email, "فعال سازی حساب کاربری", body);

                return RedirectToAction("Index", "DisplayEmail");
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

        public IActionResult ConfirmEmail(string userId, string token)
        {
            if(userId == null || token == null)
            {
                return BadRequest();
            }

            var user = _userManager.FindByIdAsync(userId).Result;
            if (user == null)
            {
                return View("Error");
            }

            var result = _userManager.ConfirmEmailAsync(user, token).Result;
            if(result.Succeeded)
            {
                //return
            }
            else
            {

            }

            return RedirectToAction("Login");
        }

        public IActionResult DisplayEmail()
        {
            return View();
        }
    }
}
