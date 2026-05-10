using EduTrack.ViewModels;

namespace EduTrack.Interfaces
{
    public interface IAdminService
    {
        List<DeletedRecordViewModel> GetDeletedRecords(string module);
        DeletedRecordViewModel? GetDeletedRecord(string module, int id);
        bool Restore(string module, int id);
        bool PermanentDelete(string module, int id);
        List<SystemBrokenRelation> FindBrokenRelations();
        bool IgnoreIssue(string module, int recordId, string reason = "");
    }
}
