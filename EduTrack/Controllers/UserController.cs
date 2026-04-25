using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Security.Claims;
using EduTrack.Constants;

namespace EduTrack.Controllers
{
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        private readonly IRoleService _roleService;
        private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        private readonly IStudentService _studentService;

        private readonly ITeacherService _teacherService;

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

            var adminRoleId = roles.FirstOrDefault(r => r.Role_Name == AppRoles.Admin)?.Role_Id;
            var teacherRoleId = roles.FirstOrDefault(r => r.Role_Name == AppRoles.Teacher)?.Role_Id;

            if (Role == AppRoles.Teacher)
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
                    u.PhoneNumber,
                    u.Address,
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

    
        // =========================
        // EDIT
        // =========================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var u = _userService.GetById(id);
            if (u == null) return NotFound();
            var roles = _roleService.GetAll();

            var studentRoleId = roles.FirstOrDefault(r => r.Role_Name == AppRoles.Student)?.Role_Id;

            if (Role == AppRoles.Teacher)
            {
                if (u.Role_Id != studentRoleId)
                {
                    return Forbid();
                }
                // Teachers should only be able to assign the Student role when editing users
                roles = roles.Where(r => r.Role_Id == studentRoleId).ToList();
            }


            var vm = new UserViewModel(
                u.User_Id,
                u.User_Name,
                u.PasswordHash,
                u.Email,
                u.PhoneNumber,
                u.Address,
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
            // Handle optional password
            if (string.IsNullOrWhiteSpace(model.PasswordHash))
            {
                ModelState.Remove(nameof(model.PasswordHash));
            }

            // Validate Role_Id early
            if (!model.Role_Id.HasValue)
            {
                ModelState.AddModelError(nameof(model.Role_Id), "Role is required");
            }

            if (!ModelState.IsValid)
            {
                LoadRoles(model.Role_Id);
                return View(model);
            }

            try
            {
                var existing = _userService.GetById(model.User_Id);
                if (existing == null) return NotFound();

                var allRoles = _roleService.GetAll();
                var studentRoleId = allRoles.FirstOrDefault(r => r.Role_Name == AppRoles.Student)?.Role_Id;

                if (Role == AppRoles.Teacher)
                {
                    if (existing.Role_Id != studentRoleId || model.Role_Id != studentRoleId)
                    {
                        return Forbid();
                    }
                }

                var role = _roleService.GetById(model.Role_Id.Value);
                if (role == null) return NotFound();

                var updated = new User(
                    existing.User_Id,
                    model.User_Name,
                    string.IsNullOrWhiteSpace(model.PasswordHash)
                        ? existing.PasswordHash
                        : PasswordHelper.HashPassword(model.PasswordHash),
                    model.Email,
                    existing.PhoneNumber,
                    existing.Address,
                    role.Role_Id,
                    role.Role_Name,
                    existing.Created_By,
                    existing.Created_Date,
                    UserId.ToString(),
                    DateTime.UtcNow,
                    model.IsActive,
                    model.IsDeleted
                );

                _userService.Update(updated);

                TempData["Success"] = "User updated successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                LoadRoles(model.Role_Id);
                return View(model);
            }
        }
        private void LoadRoles(int? selectedRoleId = null)
        {
            var roles = _roleService.GetAll();
            ViewBag.Roles = new SelectList(roles, "Role_Id", "Role_Name", selectedRoleId);
        }
        // =========================
        // DELETE
        // =========================
        [Authorize(Roles = AppRoles.Admin)]
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
                u.PhoneNumber,
                u.Address,
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

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int User_Id)
        {
            _userService.Delete(User_Id);
            TempData["Success"] = "User deleted successfully.";
            return RedirectToAction("Index");
        }
    }
}
