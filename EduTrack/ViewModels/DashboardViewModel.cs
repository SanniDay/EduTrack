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
        public int TotalClassSubjects { get; set; }
        public int TotalFeeStructures { get; set; }
        public int ActiveUsers { get; set; }
        public int ActiveTeachers { get; set; }
        public int ActiveStudents { get; set; }
        public int PendingFeesCount { get; set; }
        public int OverdueFeesCount { get; set; }
        public decimal CollectedFeesAmount { get; set; }
        public decimal PendingFeesAmount { get; set; }
        public int TodayAttendanceMarked { get; set; }
        public int PresentToday { get; set; }
        public int AbsentToday { get; set; }

        // Metrics for Teacher
        public int ActiveClasses { get; set; }
        public int EnrolledStudents { get; set; }
        public int TotalSubjects { get; set; }
        public int TimetableEntries { get; set; }
        public int TodayClassCount { get; set; }
        public int AttendanceMarkedByMeToday { get; set; }
        public string TeacherName { get; set; } = string.Empty;
        public string CurrentDateTime { get; set; } = string.Empty;

        // Metrics for Student
        public string StudentName { get; set; } = string.Empty;
        public string PrimaryClassName { get; set; } = string.Empty;
        public int AttendanceRecords { get; set; }
        public int PresentRecords { get; set; }
        public decimal AttendancePercentage { get; set; }
        public int PaidFeesCount { get; set; }
        public decimal MyPaidFeesAmount { get; set; }
        public decimal MyPendingFeesAmount { get; set; }

        // Timetable widgets
        public List<EduTrack.ViewModels.TimeTableViewModel> TodayClasses { get; set; } = new();
        public EduTrack.ViewModels.TimeTableViewModel? CurrentClass { get; set; }
        public EduTrack.ViewModels.TimeTableViewModel? NextClass { get; set; }
        public Dictionary<string, List<EduTrack.ViewModels.TimeTableViewModel>> WeeklyTimetable { get; set; } = new();
    }
}
