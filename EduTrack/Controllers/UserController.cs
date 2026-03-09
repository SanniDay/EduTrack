using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;

namespace EduTrack.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IRoleService _roleService;
        private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        public UserController(IUserService userService, IRoleService roleService)
        {
            _userService = userService;
            _roleService = roleService;
        }


        public IActionResult Index()
        {
            //Get all users from database
            List<User> lstusers = _userService.GetAll();

            // Get roles from database
            var roles = _roleService.GetAll();

            var adminRoleId = roles.FirstOrDefault(r => r.Role_Name == "Admin")?.Role_Id;
            var teacherRoleId = roles.FirstOrDefault(r => r.Role_Name == "Teacher")?.Role_Id;

            if (Role == "Teacher")
            {
                lstusers = lstusers
                    .Where(u => u.Role_Id != adminRoleId
                             && u.Role_Id != teacherRoleId
                             && u.User_Id.ToString() != UserId)
                    .ToList();
            }

            List<UserViewModel> userViewModels = lstusers
                .Select(u => new UserViewModel(
                    u.User_Id,
                    u.User_Name,
                    u.PasswordHash,
                    u.Email,
                    u.Role_Id,
                    u.Role_Name,
                    u.Created_By,
                    u.Created_Date,
                    u.Modified_By,
                    u.Modified_Date,
                    u.IsActive,
                    u.IsDeleted
                ))
                .ToList();

            return View(userViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            UserViewModel user = new UserViewModel();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Save to DB here
            return RedirectToAction("Index");
        }

        // =========================
        // EDIT
        // =========================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var u = _userService.GetById(id);
            if (u == null) return NotFound();
            var roles = _roleService.GetAll();

            var studentRoleId = roles.FirstOrDefault(r => r.Role_Name == "Student")?.Role_Id;

            if (Role == "Teacher")
            {
                roles = [.. roles.Where(u => u.Role_Id == studentRoleId)];
            }


            var vm = new UserViewModel(
                u.User_Id,
                u.User_Name,
                u.PasswordHash,
                u.Email,
                u.Role_Id,
                u.Role_Name,
                u.Created_By,
                u.Created_Date,
                u.Modified_By,
                u.Modified_Date,
                u.IsActive,
                u.IsDeleted
            );

            // Clear password field for security (do not expose the hash in the edit form)
            vm.PasswordHash = string.Empty;


            ViewBag.Roles = new SelectList(roles, "Role_Id", "Role_Name", vm.Role_Id);

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                ModelState.Remove(nameof(model.PasswordHash));
            }
            if (!ModelState.IsValid)
            {
                var roles = _roleService.GetAll();
                ViewBag.Roles = new SelectList(roles, "Role_Id", "Role_Name", model.Role_Id);
                return View(model);
            }

            var existing = _userService.GetById(model.User_Id);
            if (existing == null) return NotFound();

            // Preserve created metadata; update other fields. If password left blank, keep existing hash.
            var updated = new User(
                model.User_Id,
                model.User_Name,
                string.IsNullOrWhiteSpace(model.PasswordHash) ? existing.PasswordHash : PasswordHelper.HashPassword(model.PasswordHash),
                model.Email,
                model.Role_Id,
                string.Empty,
                existing.Created_By,
                existing.Created_Date,
                "System",
                DateTime.UtcNow,
                model.IsActive,
                model.IsDeleted
            );

            _userService.Update(updated);

            return RedirectToAction("Index");
        }

        // =========================
        // DELETE
        // =========================
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var u = _userService.GetById(id);
            if (u == null) return NotFound();

            var vm = new UserViewModel(
                u.User_Id,
                u.User_Name,
                u.PasswordHash,
                u.Email,
                u.Role_Id,
                u.Role_Name,
                u.Created_By,
                u.Created_Date,
                u.Modified_By,
                u.Modified_Date,
                u.IsActive,
                u.IsDeleted
            );

            // Provide role name for display
            if (vm.Role_Id.HasValue)
            {
                var role = _roleService.GetById(vm.Role_Id.Value);
                ViewBag.RoleName = role?.Role_Name ?? "N/A";
            }
            else
            {
                ViewBag.RoleName = "None";
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int User_Id)
        {
            _userService.Delete(User_Id);
            return RedirectToAction("Index");
        }
    }
}
