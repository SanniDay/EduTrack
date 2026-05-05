using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Services;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Security.Claims;
using EduTrack.Constants;

namespace EduTrack.Controllers
{
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Teacher}")]
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        public TeacherController(ITeacherService teacherService, IUserService userService, IRoleService roleService)
        {
            _teacherService = teacherService;
            _userService = userService;
            _roleService = roleService;
        }
        public IActionResult Index()
        {
            //Get all teacher from database
            List<Teacher> lstTeacher = _teacherService.GetAll();

            // Get roles from database
            var roles = _roleService.GetAll();

            var adminRoleId = roles.FirstOrDefault(r => r.Role_Name == AppRoles.Admin)?.Role_Id;
            var teacherRoleId = roles.FirstOrDefault(r => r.Role_Name == AppRoles.Teacher)?.Role_Id;

            if (Role == AppRoles.Teacher)
            {
                lstTeacher = lstTeacher
                    .Where(u => u.User_Id.ToString() == UserId)
                    .ToList();
            }

            List<TeacherViewModel> teacherViewModels = lstTeacher
                .Select(t => new TeacherViewModel(
                    t.Teacher_Id,
                    t.User_Id,
                    t.FullName,
                    t.Phone_No,
                    t.Address,
                    t.Created_By,
                    t.Created_Date,
                    t.Modified_By,
                    t.Modified_Date,
                    t.IsActive,
                    t.IsDeleted
                ))
                .ToList();

            return View(teacherViewModels);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var teacher = _teacherService.GetById(id);
            if (teacher == null)
            {
                return NotFound();
            }

            if (Role == AppRoles.Teacher && teacher.User_Id.ToString() != UserId)
            {
                return Forbid();
            }
            TeacherViewModel model = new TeacherViewModel(teacher.Teacher_Id, teacher.User_Id,
                teacher.FullName, teacher.Phone_No, teacher.Address, teacher.Created_By, teacher.Created_Date,
                teacher.Modified_By, teacher.Modified_Date, teacher.IsActive, teacher.IsDeleted);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TeacherViewModel model)
        {
            if (Role == AppRoles.Teacher && model.User_Id.ToString() != UserId)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                model.Modified_By = HttpContext.User.Identity?.Name ?? "System";
                model.Modified_Date = DateTime.Now;

                // Teacher table no longer stores Phone_No or Address — only FullName, IsActive, etc.
                Teacher teacher = new Teacher(model.Teacher_Id, model.User_Id, model.FullName,
                    model.Phone_No, model.Address, model.Created_By, model.Created_Date,
                    model.Modified_By, model.Modified_Date, (Role == AppRoles.Teacher) || model.IsActive, model.IsDeleted);
                _teacherService.Update(teacher);

                // Phone_No, Address and IsActive live in the User table — always sync them
                var userAccount = _userService.GetById(model.User_Id);
                if (userAccount != null)
                {
                    userAccount.IsActive = model.IsActive;
                    if(Role == AppRoles.Teacher)
                    {
                        userAccount.IsActive = true;
                    }

                    userAccount.PhoneNumber = model.Phone_No;
                    userAccount.Address = model.Address;
                    userAccount.Modified_By = model.Modified_By;
                    _userService.Update(userAccount);
                }

                TempData["Success"] = "Teacher updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var teacher = _teacherService.GetById(id);

            if (teacher == null)
            {
                return NotFound();
            }

            TeacherViewModel model = new TeacherViewModel(
                teacher.Teacher_Id,
                teacher.User_Id,
                teacher.FullName,
                teacher.Phone_No,
                teacher.Address,
                teacher.Created_By,
                teacher.Created_Date,
                teacher.Modified_By,
                teacher.Modified_Date,
                teacher.IsActive,
                teacher.IsDeleted
            );

            return View(model);
        }

        [Authorize(Roles = AppRoles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(TeacherViewModel model)
        {
            var teacher = _teacherService.GetById(model.Teacher_Id);

            if (teacher == null)
            {
                return NotFound();
            }


            teacher.IsDeleted = true;
            teacher.Modified_By = HttpContext.User.Identity.Name;
            teacher.Modified_Date = DateTime.Now;

            _teacherService.Update(teacher);
            TempData["Success"] = "Teacher deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }



}

