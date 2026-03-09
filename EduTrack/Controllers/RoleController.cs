using EduTrack.Interfaces;
using EduTrack.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers
{
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
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
        public IActionResult DeleteConfirmed(int id)
        {
            _roleService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}