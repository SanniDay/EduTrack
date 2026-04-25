using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly DbHelper _db;

        public SubjectService(DbHelper db)
        {
            _db = db;
        }

        public List<Subject> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_Subject_GetAll");
            return ToList(dt);
        }

        public Subject? GetById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Subject_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_Subject_GetById", parameters);

            if (dt.Rows.Count == 0) return null;

            return Map(dt.Rows[0]);
        }

        public void Create(Subject subject)
        {
            var parameters = new[]
            {
                new SqlParameter("@Subject_Name", subject.Subject_Name),
                new SqlParameter("@Subject_Code", subject.Subject_Code),
                new SqlParameter("@Description", string.IsNullOrWhiteSpace(subject.Description) ? (object)DBNull.Value : subject.Description),
                new SqlParameter("@Created_By", subject.Created_By)
            };

            var result = _db.ExecuteProcedureScalar("sp_Subject_Create", parameters);
            if (result != null && int.TryParse(result.ToString(), out int newId))
            {
                subject.Subject_Id = newId;
            }
        }

        public void Update(Subject subject)
        {
            var parameters = new[]
            {
                new SqlParameter("@Subject_Id", subject.Subject_Id),
                new SqlParameter("@Subject_Name", subject.Subject_Name),
                new SqlParameter("@Subject_Code", subject.Subject_Code),
                new SqlParameter("@Description", subject.Description),
                new SqlParameter("@Modified_By", subject.Modified_By),
                new SqlParameter("@IsActive", subject.IsActive)
            };

            _db.ExecuteProcedureNonQuery("sp_Subject_Update", parameters);
        }

        public void Delete(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Subject_Id", id),
                new SqlParameter("@Modified_By", "System")
            };

            _db.ExecuteProcedureNonQuery("sp_Subject_Delete", parameters);
        }

        private List<Subject> ToList(DataTable dt)
        {
            List<Subject> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        private Subject Map(DataRow row)
        {
            return new Subject(
                Convert.ToInt32(row["Subject_Id"]),
                row["Subject_Name"]?.ToString() ?? "",
                row["Subject_Code"]?.ToString() ?? "",
                row["Description"]?.ToString() ?? "",
                row["Created_By"]?.ToString() ?? "",
                row["Created_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created_Date"],
                row["Modified_By"]?.ToString() ?? "",
                row["Modified_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Modified_Date"],
                row["IsActive"] == DBNull.Value ? false : (bool)row["IsActive"],
                row["IsDeleted"] == DBNull.Value ? false : (bool)row["IsDeleted"]
            );
        }
    }
}
