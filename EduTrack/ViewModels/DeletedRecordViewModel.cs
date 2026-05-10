using System;

namespace EduTrack.ViewModels
{
    public class DeletedRecordViewModel
    {
        public string Module { get; set; } = string.Empty;
        public int RecordId { get; set; }
        public string DisplayText { get; set; } = string.Empty;
        public string Details { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime? DeletedDate { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
