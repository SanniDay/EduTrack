using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.ViewModels;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EduTrack.Services
{
    public class AdminService : IAdminService
    {
        private readonly DbHelper _db;
        private readonly ILogger<AdminService> _logger;

        public AdminService(DbHelper db, ILogger<AdminService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public List<DeletedRecordViewModel> GetDeletedRecords(string module)
        {
            var list = new List<DeletedRecordViewModel>();
            try
            {
                var proc = $"sp_{module}_GetAll_Admin";
                DataTable dt = _db.ExecuteProcedure(proc);
                foreach (DataRow row in dt.Rows)
                {
                    var idCol = dt.Columns.Contains($"{module}_Id") ? $"{module}_Id" : dt.Columns.Contains("Id") ? "Id" : dt.Columns[0].ColumnName;
                    int id = row[idCol] != DBNull.Value ? Convert.ToInt32(row[idCol]) : 0;

                    // Module-specific display text
                    string display = id.ToString();
                    var mod = module?.ToLowerInvariant() ?? string.Empty;
                    switch (mod)
                    {
                        case "student":
                            if (dt.Columns.Contains("FullName")) display = row["FullName"]?.ToString() ?? id.ToString();
                            else if (dt.Columns.Contains("FirstName") || dt.Columns.Contains("LastName"))
                            {
                                var f = dt.Columns.Contains("FirstName") ? row["FirstName"]?.ToString() : string.Empty;
                                var l = dt.Columns.Contains("LastName") ? row["LastName"]?.ToString() : string.Empty;
                                display = string.Join(' ', new[] { f, l }.Where(s => !string.IsNullOrWhiteSpace(s)));
                                if (string.IsNullOrWhiteSpace(display)) display = id.ToString();
                            }
                            break;
                        case "teacher":
                            if (dt.Columns.Contains("FullName")) display = row["FullName"]?.ToString() ?? id.ToString();
                            break;
                        case "class":
                            var cname = dt.Columns.Contains("ClassName") ? row["ClassName"]?.ToString() : null;
                            var section = dt.Columns.Contains("Section") ? row["Section"]?.ToString() : null;
                            display = (!string.IsNullOrWhiteSpace(cname) ? cname : id.ToString()) + (string.IsNullOrWhiteSpace(section) ? string.Empty : " - " + section);
                            break;
                        case "subject":
                            if (dt.Columns.Contains("Subject_Name")) display = row["Subject_Name"]?.ToString() ?? id.ToString();
                            break;
                        case "role":
                            if (dt.Columns.Contains("Role_Name")) display = row["Role_Name"]?.ToString() ?? id.ToString();
                            break;
                        case "user":
                            if (dt.Columns.Contains("UserName")) display = row["UserName"]?.ToString() ?? id.ToString();
                            else if (dt.Columns.Contains("Email")) display = row["Email"]?.ToString() ?? id.ToString();
                            break;
                        case "fees":
                        case "fee":
                            if (dt.Columns.Contains("Fees_Name")) display = row["Fees_Name"]?.ToString() ?? id.ToString();
                            else if (dt.Columns.Contains("Description")) display = row["Description"]?.ToString() ?? id.ToString();
                            break;
                        case "timetable":
                            var day = dt.Columns.Contains("Day_Name") ? row["Day_Name"]?.ToString() : null;
                            var per = dt.Columns.Contains("Period_No") ? row["Period_No"]?.ToString() : null;
                            display = (!string.IsNullOrWhiteSpace(day) ? day : "") + (string.IsNullOrWhiteSpace(per) ? string.Empty : " - Period " + per);
                            break;
                        default:
                            if (dt.Columns.Contains("FullName")) display = row["FullName"]?.ToString() ?? id.ToString();
                            else if (dt.Columns.Contains("Name")) display = row["Name"]?.ToString() ?? id.ToString();
                            else if (dt.Columns.Contains("Description")) display = row["Description"]?.ToString() ?? id.ToString();
                            else display = id.ToString();
                            break;
                    }
                    bool isDeleted = row.Table.Columns.Contains("IsDeleted") ? (row["IsDeleted"] != DBNull.Value && (bool)row["IsDeleted"]) : true;
                    var deletedBy = row.Table.Columns.Contains("Modified_By") ? row["Modified_By"]?.ToString() ?? string.Empty : row.Table.Columns.Contains("Created_By") ? row["Created_By"]?.ToString() ?? string.Empty : string.Empty;
                    DateTime? deletedDate = null;
                    if (row.Table.Columns.Contains("Modified_Date") && row["Modified_Date"] != DBNull.Value)
                        deletedDate = (DateTime)row["Modified_Date"];

                    list.Add(new DeletedRecordViewModel
                    {
                        Module = module,
                        RecordId = id,
                        DisplayText = display,
                        Details = row.Table.Columns.Contains("Details") ? row["Details"]?.ToString() ?? string.Empty : string.Empty,
                        IsDeleted = isDeleted,
                        Status = isDeleted ? "Deleted" : "Active",
                        DeletedBy = deletedBy,
                        DeletedDate = deletedDate
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deleted records for module {Module}", module);
            }
            return list;
        }

        public bool IgnoreIssue(string module, int recordId, string reason = "")
        {
            try
            {
                // Currently ignore is non-persistent: log and return success.
                _logger.LogInformation("Ignored issue on {Module}:{Id}. Reason: {Reason}", module, recordId, reason);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to ignore issue {Module}:{Id}", module, recordId);
                return false;
            }
        }

        public DeletedRecordViewModel? GetDeletedRecord(string module, int id)
        {
            try
            {
                var proc = $"sp_{module}_GetById";
                var parameters = new[] { new SqlParameter($"@{module}_Id", id) };
                DataTable dt = _db.ExecuteProcedure(proc, parameters);
                if (dt.Rows.Count == 0) return null;
                var row = dt.Rows[0];
                // Module-specific display
                var display = id.ToString();
                var mod = module?.ToLowerInvariant() ?? string.Empty;
                switch (mod)
                {
                    case "student":
                        if (dt.Columns.Contains("FullName")) display = row["FullName"]?.ToString() ?? id.ToString();
                        break;
                    case "teacher":
                        if (dt.Columns.Contains("FullName")) display = row["FullName"]?.ToString() ?? id.ToString();
                        break;
                    case "class":
                        var cname = dt.Columns.Contains("ClassName") ? row["ClassName"]?.ToString() : null;
                        var section = dt.Columns.Contains("Section") ? row["Section"]?.ToString() : null;
                        display = (!string.IsNullOrWhiteSpace(cname) ? cname : id.ToString()) + (string.IsNullOrWhiteSpace(section) ? string.Empty : " - " + section);
                        break;
                    case "subject":
                        if (dt.Columns.Contains("Subject_Name")) display = row["Subject_Name"]?.ToString() ?? id.ToString();
                        break;
                    case "role":
                        if (dt.Columns.Contains("Role_Name")) display = row["Role_Name"]?.ToString() ?? id.ToString();
                        break;
                    case "user":
                        if (dt.Columns.Contains("UserName")) display = row["UserName"]?.ToString() ?? id.ToString();
                        else if (dt.Columns.Contains("Email")) display = row["Email"]?.ToString() ?? id.ToString();
                        break;
                    default:
                        if (dt.Columns.Contains("FullName")) display = row["FullName"]?.ToString() ?? id.ToString();
                        else if (dt.Columns.Contains("Name")) display = row["Name"]?.ToString() ?? id.ToString();
                        else if (dt.Columns.Contains("Description")) display = row["Description"]?.ToString() ?? id.ToString();
                        else display = id.ToString();
                        break;
                }
                return new DeletedRecordViewModel
                {
                    Module = module,
                    RecordId = id,
                    DisplayText = display,
                    Details = row.Table.Columns.Contains("Details") ? row["Details"]?.ToString() ?? string.Empty : string.Empty,
                    IsDeleted = row.Table.Columns.Contains("IsDeleted") ? (row["IsDeleted"] != DBNull.Value && (bool)row["IsDeleted"]) : true,
                    DeletedBy = row.Table.Columns.Contains("Modified_By") ? row["Modified_By"]?.ToString() ?? string.Empty : string.Empty,
                    DeletedDate = row.Table.Columns.Contains("Modified_Date") && row["Modified_Date"] != DBNull.Value ? (DateTime?)row["Modified_Date"] : null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching deleted record {Module}:{Id}", module, id);
                return null;
            }
        }

        public bool Restore(string module, int id)
        {
            try
            {
                var proc = $"sp_{module}_Restore";
                var parameters = new[] { new SqlParameter($"@{module}_Id", id), new SqlParameter("@Modified_By", "Admin") };
                _db.ExecuteProcedureNonQuery(proc, parameters);
                _logger.LogInformation("Restored {Module}:{Id}", module, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to restore {Module}:{Id}", module, id);
                return false;
            }
        }

        public bool PermanentDelete(string module, int id)
        {
            try
            {
                // Check for broken relations that reference this record
                var broken = FindBrokenRelations();
                var related = broken.Where(b => string.Equals(b.MissingModule, module, StringComparison.OrdinalIgnoreCase) && b.MissingId == id).ToList();
                if (related.Any())
                {
                    _logger.LogWarning("Cannot permanently delete {Module}:{Id} because {Count} dependent references exist", module, id, related.Count);
                    return false;
                }

                var proc = $"sp_{module}_PermanentDelete";
                var parameters = new[] { new SqlParameter($"@{module}_Id", id) };
                _db.ExecuteProcedureNonQuery(proc, parameters);
                _logger.LogInformation("Permanently deleted {Module}:{Id}", module, id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to permanently delete {Module}:{Id}", module, id);
                return false;
            }
        }

        public List<SystemBrokenRelation> FindBrokenRelations()
        {
            var list = new List<SystemBrokenRelation>();
            try
            {
                DataTable dt = _db.ExecuteProcedure("sp_System_FindBrokenRelations");
                foreach (DataRow row in dt.Rows)
                {
                    var issue = new SystemBrokenRelation();
                    issue.IssueType = row.Table.Columns.Contains("IssueType") ? row["IssueType"]?.ToString() ?? string.Empty : row.Table.Columns.Contains("Issue") ? row["Issue"]?.ToString() ?? string.Empty : "Relation";
                    issue.Module = row.Table.Columns.Contains("Module") ? row["Module"]?.ToString() ?? string.Empty : row.Table.Columns.Contains("ReferencingModule") ? row["ReferencingModule"]?.ToString() ?? string.Empty : string.Empty;
                    issue.RecordId = row.Table.Columns.Contains("RecordId") && row["RecordId"] != DBNull.Value ? Convert.ToInt32(row["RecordId"]) : row.Table.Columns.Contains("ReferencingId") && row["ReferencingId"] != DBNull.Value ? Convert.ToInt32(row["ReferencingId"]) : 0;
                    issue.MissingModule = row.Table.Columns.Contains("MissingModule") ? row["MissingModule"]?.ToString() ?? string.Empty : row.Table.Columns.Contains("ReferencedModule") ? row["ReferencedModule"]?.ToString() ?? string.Empty : string.Empty;
                    issue.MissingId = row.Table.Columns.Contains("MissingId") && row["MissingId"] != DBNull.Value ? Convert.ToInt32(row["MissingId"]) : row.Table.Columns.Contains("ReferencedId") && row["ReferencedId"] != DBNull.Value ? Convert.ToInt32(row["ReferencedId"]) : 0;
                    issue.Description = row.Table.Columns.Contains("Description") ? row["Description"]?.ToString() ?? string.Empty : string.Empty;
                    issue.Severity = row.Table.Columns.Contains("Severity") ? row["Severity"]?.ToString() ?? string.Empty : "Warning";
                    list.Add(issue);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error running sp_System_FindBrokenRelations");
            }
            return list;
        }
    }
}
