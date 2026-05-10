using EduTrack.Constants;
using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduTrack.Controllers.Admin
{
    [Authorize(Roles = AppRoles.Admin)]
    public class SystemIntegrityController : Controller
    {
        private readonly IAdminService _adminService;
        private readonly ILogger<SystemIntegrityController> _logger;

        public SystemIntegrityController(IAdminService adminService, ILogger<SystemIntegrityController> logger)
        {
            _adminService = adminService;
            _logger = logger;
        }

        public IActionResult Index(string module = "", string severity = "", string issueType = "", string search = "", int page = 1)
        {
            var vm = new SystemIntegrityViewModel();
            var issues = _adminService.FindBrokenRelations();

            if (!string.IsNullOrWhiteSpace(module))
                issues = issues.Where(i => string.Equals(i.Module, module, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrWhiteSpace(severity))
                issues = issues.Where(i => string.Equals(i.Severity, severity, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrWhiteSpace(issueType))
                issues = issues.Where(i => string.Equals(i.IssueType, issueType, StringComparison.OrdinalIgnoreCase)).ToList();
            if (!string.IsNullOrWhiteSpace(search))
                issues = issues.Where(i => (i.Description ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) || i.RecordId.ToString() == search || i.MissingId.ToString() == search).ToList();

            vm.TotalIssues = issues.Count;
            vm.CriticalCount = issues.Count(i => string.Equals(i.Severity, "Critical", StringComparison.OrdinalIgnoreCase));
            vm.WarningCount = issues.Count(i => string.Equals(i.Severity, "Warning", StringComparison.OrdinalIgnoreCase));
            vm.InfoCount = issues.Count(i => string.Equals(i.Severity, "Info", StringComparison.OrdinalIgnoreCase));

            // pagination simple
            int pageSize = 25;
            vm.Issues = issues.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            vm.ModuleFilter = module;
            vm.SeverityFilter = severity;
            vm.IssueTypeFilter = issueType;
            vm.Search = search;

            return View("~/Views/Admin/SystemIntegrity/Index.cshtml", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RestoreReferenced(string module, int missingId)
        {
            if (string.IsNullOrWhiteSpace(module) || missingId <= 0) return BadRequest();
            var ok = _adminService.Restore(module, missingId);
            if (ok) TempData["Message"] = "Referenced record restored."; else TempData["Error"] = "Failed to restore referenced record.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteOrphan(string module, int recordId)
        {
            if (string.IsNullOrWhiteSpace(module) || recordId <= 0) return BadRequest();
            var ok = _adminService.PermanentDelete(module, recordId);
            if (ok) TempData["Message"] = "Orphan record permanently deleted."; else TempData["Error"] = "Failed to delete orphan record or dependencies exist.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Ignore(string module, int recordId, string reason = "")
        {
            if (string.IsNullOrWhiteSpace(module) || recordId <= 0) return BadRequest();
            var ok = _adminService.IgnoreIssue(module, recordId, reason);
            if (ok) TempData["Message"] = "Issue ignored."; else TempData["Error"] = "Failed to ignore issue.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult BulkFix(string actionType, [FromForm] List<int> recordIds, [FromForm] List<string> modules)
        {
            if (recordIds == null || modules == null || recordIds.Count != modules.Count)
            {
                return BadRequest();
            }

            int success = 0, failed = 0;
            for (int i = 0; i < recordIds.Count; i++)
            {
                var id = recordIds[i];
                var module = modules[i];
                try
                {
                    if (actionType == "restore") { if (_adminService.Restore(module, id)) success++; else failed++; }
                    else if (actionType == "delete") { if (_adminService.PermanentDelete(module, id)) success++; else failed++; }
                    else if (actionType == "ignore") { if (_adminService.IgnoreIssue(module, id, "bulk ignore")) success++; else failed++; }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "BulkFix error for {Module}:{Id}", module, id);
                    failed++;
                }
            }

            return Json(new { success, failed });
        }

        [HttpGet]
        public IActionResult ExportCsv()
        {
            var issues = _adminService.FindBrokenRelations();
            var csv = new System.Text.StringBuilder();
            csv.AppendLine("IssueType,Module,RecordId,MissingModule,MissingId,Description,Severity");
            foreach (var i in issues)
            {
                var desc = (i.Description ?? string.Empty).Replace("\"", "\"\"");
                var line = $"\"{i.IssueType}\",\"{i.Module}\",{i.RecordId},\"{i.MissingModule}\",{i.MissingId},\"{desc}\",\"{i.Severity}\"";
                csv.AppendLine(line);
            }
            var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
            return File(bytes, "text/csv", "system_integrity_issues.csv");
        }
    }
}
