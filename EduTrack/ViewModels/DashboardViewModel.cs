namespace EduTrack.ViewModels
{
    public class DashboardViewModel
    {
        public string UserRole { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        // Metrics for Admin
        public int TotalUsers { get; set; }
        public int TotalTeachers { get; set; }
        public int TotalStudents { get; set; }
        public int TotalRoles { get; set; }
        public int TotalClasses { get; set; }

        // Metrics for Teacher
        public int ActiveClasses { get; set; }
        public int EnrolledStudents { get; set; }
        public int TotalSubjects { get; set; }
        public string CurrentDateTime { get; set; } = string.Empty;

        // Timetable widgets
        public List<EduTrack.ViewModels.TimeTableViewModel> TodayClasses { get; set; } = new();
        public EduTrack.ViewModels.TimeTableViewModel? CurrentClass { get; set; }
        public EduTrack.ViewModels.TimeTableViewModel? NextClass { get; set; }
        public Dictionary<string, List<EduTrack.ViewModels.TimeTableViewModel>> WeeklyTimetable { get; set; } = new();
    }
}
