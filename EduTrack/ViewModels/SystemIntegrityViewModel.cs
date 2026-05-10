using System.Collections.Generic;

namespace EduTrack.ViewModels
{
    public class SystemIntegrityViewModel
    {
        public List<SystemBrokenRelation> Issues { get; set; } = new();
        public string ModuleFilter { get; set; } = string.Empty;
        public string SeverityFilter { get; set; } = string.Empty;
        public string IssueTypeFilter { get; set; } = string.Empty;
        public string Search { get; set; } = string.Empty;

        // Summary counts
        public int TotalIssues { get; set; }
        public int CriticalCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
    }
}
