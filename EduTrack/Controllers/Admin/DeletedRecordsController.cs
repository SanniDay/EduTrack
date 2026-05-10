using EduTrack.Constants;
using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers.Admin
{
    [Authorize(Roles = AppRoles.Admin)]
    public class DeletedRecordsController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<DeletedRecordsController> _logger;

        public DeletedRecordsController(IAdminService adminService, ILogger<DeletedRecordsController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        // GET: /Admin/DeletedRecords
        public IActionResult Index(string module = "Student", string search = "", int page = 1)
        {
            var modules = new List<string>
            {
                "Student","Teacher","Attendance","Subject","Class","Fees","User","Role","TimeTable","StudentFees","StudentClass","TeacherClass","ClassSubject"
            };

            ViewBag.Modules = modules;
            ViewBag.SelectedModule = module;

            var items = _adminService.GetDeletedRecords(module);
            if (!string.IsNullOrWhiteSpace(search))
            {
                items = items.Where(i => (i.DisplayText ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // simple pagination
            int pageSize = 25;
            var paged = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.Total = items.Count;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;

            return View("~/Views/Admin/DeletedRecords/Index.cshtml", paged);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restore(string module, int id)
        {
            if (string.IsNullOrWhiteSpace(module) || id <= 0) return BadRequest();

            var ok = _adminService.Restore(module, id);
            if (ok)
            {
                TempData["Message"] = "Record restored successfully.";
            }
            else
            {
                TempData["Error"] = "Failed to restore record.";
            }
            return RedirectToAction(nameof(Index), new { module });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PermanentDelete(string module, int id)
        {
            if (string.IsNullOrWhiteSpace(module) || id <= 0) return BadRequest();

            var ok = _adminService.PermanentDelete(module, id);
            if (ok)
            {
                TempData["Message"] = "Record permanently deleted.";
            }
            else
            {
                TempData["Error"] = "Permanent delete failed or dependencies exist.";
            }
            return RedirectToAction(nameof(Index), new { module });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BulkAction(string module, string actionType, [FromForm] List<int> ids)
        {
            if (string.IsNullOrWhiteSpace(module) || ids == null || !ids.Any())
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = 0, failed = 0, error = "No items selected." });
                TempData["Error"] = "No items selected.";
                return RedirectToAction(nameof(Index));
            }

            int success = 0; int failed = 0;
            foreach (var id in ids)
            {
                try
                {
                    if (actionType == "restore")
                    {
                        if (_adminService.Restore(module, id)) success++; else failed++;
                    }
                    else if (actionType == "permanentdelete")
                    {
                        if (_adminService.PermanentDelete(module, id)) success++; else failed++;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "BulkAction error for {Module}:{Id}", module, id);
                    failed++;
                }
            }
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success, failed });
            }
            TempData["Message"] = $"Bulk action completed. Success: {success}, Failed: {failed}";
            return RedirectToAction(nameof(Index), new { module });
        }

        [HttpGet]
        public IActionResult CheckDependencies(string module, int id)
        {
            if (string.IsNullOrWhiteSpace(module) || id <= 0) return Json(new { hasDependencies = false, count = 0 });
            var broken = _adminService.FindBrokenRelations();
            var related = broken.Where(b => string.Equals(b.MissingModule, module, StringComparison.OrdinalIgnoreCase) && b.MissingId == id).ToList();
            return Json(new { hasDependencies = related.Any(), count = related.Count, related });
        }

        public IActionResult Details(string module, int id)
        {
            var item = _adminService.GetDeletedRecord(module, id);
            if (item == null) return NotFound();
            return View("~/Views/Admin/DeletedRecords/Details.cshtml", item);
        }
    }
}
