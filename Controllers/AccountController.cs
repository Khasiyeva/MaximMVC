using MaximMVC.Models;
using MaximMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MaximMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM, string? returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser() 
            { 
                Name=registerVM.Name,
                Surname=registerVM.Surname,
                UserName=registerVM.Username,
                Email = registerVM.Email
            };

            var result = await _userManager.CreateAsync(user,registerVM.Password);

            if (!result.Succeeded)
            {
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(" ", error.Description);
                }
            }

            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(loginVM.UsernameOrEmail);
            if(user == null)
            {
                _userManager.FindByNameAsync(loginVM.UsernameOrEmail);

                if (user == null)
                {
                    ModelState.AddModelError(" ", "Not Found");
                    return View();
                }

            }

            var result = _signInManager.CheckPasswordSignInAsync(user, loginVM.Password, true).Result;
            if(!result.Succeeded)
            {
                ModelState.AddModelError(" ", "Not Found");
            }

            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Waiting");
                return View();
            }
           
           
            return RedirectToAction("Login");
        }

        public IActionResult LogOut()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
