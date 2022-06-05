using VacationManagment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace VacationManagment.Controllers
{
  
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly INotyfService _notyf;

        public UserController(UserManager<IdentityUser> userManager , SignInManager<IdentityUser> signInManager , INotyfService notyf)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _notyf = notyf;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user!=null)
                {
                  var Result = await _signInManager.PasswordSignInAsync(user , model.Password ,false , false);
                    if (Result.Succeeded)
                    {
                        _notyf.Success("Login Success");
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("SignIn" , "Password or User Name is Wrong");
                    }
                }
                else
                {
                    ModelState.AddModelError("SignIn", "you are not valid");
                }
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var Result = await _userManager.CreateAsync(new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                }, model.Password);
                if (Result.Succeeded)
                {
                    _notyf.Success("Register Success");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in Result.Errors)
                    {
                        ModelState.AddModelError("CreateUser", item.Description);
                    }
                }
            }
            return View(model);

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
