using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Services;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EduTrack.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _teacherService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private string Role => User.FindFirstValue(ClaimTypes.Role) ?? "";
        private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "";

        public TeacherController(ITeacherService teacherService , IUserService userService, IRoleService roleService)
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

            var adminRoleId = roles.FirstOrDefault(r => r.Role_Name == "Admin")?.Role_Id;
            var teacherRoleId = roles.FirstOrDefault(r => r.Role_Name == "Teacher")?.Role_Id;

            if (Role == "Teacher")
            {
                lstTeacher = lstTeacher 
                    .Where(u => u.Teacher_Id != adminRoleId
                             && u.User_Id.ToString() == UserId)
                    .ToList();
            }

            List<TeacherViewModel> teacherViewModels = lstTeacher
                .Select(t => new TeacherViewModel(
                    t.User_Id,
                    t.Teacher_Id,
                    t.FullName,
                    t.Phone_No,
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
    }
}
