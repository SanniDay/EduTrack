namespace EduTrack.ViewModels
{
    public class SystemBrokenRelation
    {
        public string IssueType { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string MissingModule { get; set; } = string.Empty;
        public int MissingId { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;
    }
}
