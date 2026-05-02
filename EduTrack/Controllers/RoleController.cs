using EduTrack.Interfaces;
using EduTrack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EduTrack.Constants;

namespace EduTrack.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IWebHostEnvironment _env;

        public RoleController(IRoleService roleService, IWebHostEnvironment env)
        {
            _roleService = roleService;
            _env = env;
        }

        public IActionResult Index()
        {
            List<Role> roles = _roleService.GetAll();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Role());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Created_By = "System";
            model.Created_Date = DateTime.UtcNow;
            model.Modified_By = "System";
            model.Modified_Date = DateTime.UtcNow;
            model.IsActive = true;
            model.IsDeleted = false;

            _roleService.Create(model);
            TempData["Success"] = "Role created successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role model)
        {
            if (!ModelState.IsValid)
                return View(model);

            model.Modified_By = "System";
            model.Modified_Date = DateTime.UtcNow;

            _roleService.Update(model);
            TempData["Success"] = "Role updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var role = _roleService.GetById(id);
            if (role == null) return NotFound();
            return View(role);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int Role_Id)
        {
            // Ensure a valid id was provided
            if (Role_Id <= 0)
            {
                TempData["Error"] = "Invalid role id.";
                return RedirectToAction("Index");
            }

            try
            {
                _roleService.Delete(Role_Id);
                TempData["Success"] = "Role deleted successfully.";
            }
            catch (Exception ex)
            {
                // Log exception in real app
                if (_env.IsDevelopment())
                {
                    TempData["Error"] = "Error deleting role: " + ex.Message;
                }
                else
                {
                    TempData["Error"] = "This role cannot be deleted at this time. It may be associated with existing users or system components.";
                }
            }

            return RedirectToAction("Index");
        }
    }
}