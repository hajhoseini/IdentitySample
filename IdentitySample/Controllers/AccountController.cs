using IdentitySample.Models.DTOs;
using IdentitySample.Models.DTOs.Account;
using IdentitySample.Models.Entities;
using IdentitySample.Services;
using Microsoft.AspNetCore.Authorization;
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

        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgetPassword(ForgetPasswordConfirmationDTO dto)
        {
            if(!ModelState.IsValid)
            {
                return View(dto);
            }

            var user = _userManager.FindByEmailAsync(dto.Email).Result;
            if(user == null || _userManager.IsEmailConfirmedAsync(user).Result == false)
            {
                ViewBag.message = "ممکن است ایمیل وارد شده معتبر نباشد و یا ایمیل خود را تایید نکرده باشید";
                return View();
            }

            string token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            string callBackUrl = Url.Action("ResetPassword", "Account", new { UserId = user.Id, Token = token }, protocol: Request.Scheme);
            string body = $"برای تنظیم مجدد کلمه عبور بر روی لینک زیر کلیک کنید <br/> <a href = {callBackUrl}>Link Reset Password</a>";
            _emailService.Execute(user.Email, "فراموشی کلمه عبور", body);
            ViewBag.message = "لینک تنظیم مجدد کلمه عبور برای ایمیل شما ارسال شد";

            return View();
        }

        public IActionResult ResetPassword(string userId, string token)
        {
            var dto = new ResetPasswordDTO { UserId = userId, Token = token };
            return View(dto);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if(dto.Password !=  dto.ConfirmPassword)
            {
                return BadRequest();
            }

            var user = _userManager.FindByIdAsync(dto.UserId).Result;
            if (user == null)
            {
                return BadRequest();
            }

            var result = _userManager.ResetPasswordAsync(user, dto.Token, dto.Password).Result;

            if(result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }
            else
            {
                ViewBag.Errors = result.Errors;
                return View(dto);
            }            
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [Authorize]
        public IActionResult SetPhoneNumber()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult SetPhoneNumber(SetPhoneNumberDTO dto)
        {
            string currentUser = User.Identity.Name;
            var user = _userManager.FindByNameAsync(currentUser).Result;
            var result = _userManager.SetPhoneNumberAsync(user, dto.PhoneNumber).Result;
            string code = _userManager.GenerateChangePhoneNumberTokenAsync(user, dto.PhoneNumber).Result;

            SMSService smsService = new SMSService();
            smsService.Send(dto.PhoneNumber, code);

            TempData["PhoneNumber"] = dto.PhoneNumber;

            return RedirectToAction(nameof(VerifyPhoneNumber));
        }

        [Authorize]
        public IActionResult VerifyPhoneNumber()
        {
            VerifyPhoneNumberDTO dto = new VerifyPhoneNumberDTO
            {
                PhoneNumber = TempData["PhoneNumber"].ToString()
            };

            return View(dto);
        }

        [Authorize]
        [HttpPost]
        public IActionResult VerifyPhoneNumber(VerifyPhoneNumberDTO dto)
        {
            string currentUser = User.Identity.Name;
            var user = _userManager.FindByNameAsync(currentUser).Result;
            bool result = _userManager.VerifyChangePhoneNumberTokenAsync(user, dto.Code, dto.PhoneNumber).Result;
            if(!result)
            {
                ViewData["Message"] = $"کد وارد شده برای شماره {dto.PhoneNumber} اشتباه است";
                return View(dto);
            }
            else
            {
                user.PhoneNumberConfirmed = true;
                _userManager.UpdateAsync(user);
            }

            return RedirectToAction(nameof(VerifySuccess));
        }

        public IActionResult VerifySuccess()
        {
            return View();
        }
    }
}
