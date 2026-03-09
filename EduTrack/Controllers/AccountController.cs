using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using EduTrack.Models;
using System.Linq;

namespace EduTrack.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public AccountController(IAccountService accountService, IUserService userService, IRoleService roleService)
        {
            _accountService = accountService;
            _userService = userService;
            _roleService = roleService;
        }

        // =========================
        // REGISTER
        // =========================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            { return View(model); }

            bool result = _accountService.Register(model);

            if (!result)
            {
                ModelState.AddModelError("", "Registration failed.");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        // =========================
        // LOGIN
        // =========================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool isValid = _accountService.Authenticate(model.Email, model.Password);

            if (!isValid)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(model);
            }

            // Build claims including role if available
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email)
            };

            // Try to locate the user and attach role claim (if any)
            var user = _userService.GetAll().FirstOrDefault(u => string.Equals(u.Email, model.Email, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                // Add identifier
                claims.Add(new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString()));

                if (user.Role_Id.HasValue)
                {
                    var role = _roleService.GetById(user.Role_Id.Value);
                    if (role != null && !string.IsNullOrWhiteSpace(role.Role_Name))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Role_Name));
                    }
                }
            }

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                });

            return RedirectToAction("Index", "Home");
        }

        // =========================
        // LOGOUT
        // =========================

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Login");
        }

        // =========================
        // ACCESS DENIED
        // =========================

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}