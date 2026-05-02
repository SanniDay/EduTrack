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
        private readonly IEmailService _emailService;

        public AccountController(IAccountService accountService, IUserService userService, IRoleService roleService, IEmailService emailService)
        {
            _accountService = accountService;
            _userService = userService;
            _roleService = roleService;
            _emailService = emailService;
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
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            { return View(model); }

            var registrationResult = _accountService.Register(model);

            if (!registrationResult.Success)
            {
                ModelState.AddModelError("", registrationResult.Message);
                return View(model);
            }

            // Auto-login the newly registered user
            var user = _userService.GetAll().FirstOrDefault(u => string.Equals(u.Email, model.Email, StringComparison.OrdinalIgnoreCase));

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, model.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.User_Id.ToString())
                };

                if (user.Role_Id.HasValue)
                {
                    var role = _roleService.GetById(user.Role_Id.Value);
                    if (role != null && !string.IsNullOrWhiteSpace(role.Role_Name))
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Role_Name));
                    }
                }

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = false
                    });
            }

            // Send registration confirmation email (non-blocking)
            try
            {
                var to = model.Email;
                var subject = "EduTrack - Registration Received";
                var html = $@"
                    <p>Dear {model.FullName},</p>
                    <p>Thank you for registering with EduTrack.</p>
                    <p>Your account has been created and is pending activation. You will receive an email confirmation once your account is activated (typically within 3-4 business days). Please wait until you receive the confirmation before attempting to access role-specific features.</p>
                    <p>If you have any questions, reply to this email or contact support at <strong>support@edutrack.local</strong>.</p>
                    <p>Regards,<br/>EduTrack Team</p>
                ";

                // fire-and-forget (don't block user flow)
                _ = _emailService.SendEmailAsync(to, subject, html);
            }
            catch
            {
                // swallow - email failure should not block registration
            }

            return RedirectToAction("Index", "Home");
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

            var authResult = _accountService.Authenticate(model.Email, model.Password);

            if (!authResult.Success)
            {
                ModelState.AddModelError("", authResult.Message);
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