using IdentitySample.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentitySample.Controllers
{
    public class UserClaimController : Controller
    {
        private readonly UserManager<User> _userManager;

        public UserClaimController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            return View(User.Claims);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string claimType, string claimValue)
        {
            var user = _userManager.GetUserAsync(User).Result;
            Claim newCliam = new Claim(claimType, claimValue, ClaimValueTypes.String);

            var result = _userManager.AddClaimAsync(user, newCliam).Result;
            if(result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach(var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }

            return View();
        }

        public IActionResult Delete(string claimType)
        {
            var user = _userManager.GetUserAsync(User).Result;
            Claim claim = User.Claims.Where(p => p.Type == claimType).FirstOrDefault();
            if(claim != null)
            {
                var result = _userManager.RemoveClaimAsync(user, claim).Result;
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
    }
}