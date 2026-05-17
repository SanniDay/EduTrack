using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;

namespace EduTrack.Services
{
    public class ClassSubjectService : IClassSubjectService
    {
        private readonly DbHelper _db;

        public ClassSubjectService(DbHelper db)
        {
            _db = db;
        }

        // =============================
        // GET ALL
        // =============================
        public List<ClassSubject> GetAllClassSubjects()
        {
            DataTable dt = _db.ExecuteProcedure("sp_ClassSubject_GetAll");

            List<ClassSubject> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(Map(row));
            }

            return list;
        }

        // =============================
        // GET BY ID
        // =============================
        public ClassSubject? GetClassSubjectById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClassSubject_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_ClassSubject_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return Map(dt.Rows[0]);
        }

        // =============================
        // CREATE
        // =============================
        public void CreateClassSubject(ClassSubject cs)
        {
            var parameters = new[]
            {
                new SqlParameter("@Class_Id", cs.Class_Id),
                new SqlParameter("@Subject_Id", cs.Subject_Id),
                new SqlParameter("@IsCore", cs.IsCore),
                new SqlParameter("@IsActive", cs.IsActive),
                new SqlParameter("@Created_By", cs.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_ClassSubject_Create", parameters);
        }

        // =============================
        // UPDATE
        // =============================
        public void UpdateClassSubject(ClassSubject cs)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClassSubject_Id", cs.ClassSubject_Id),
                new SqlParameter("@Class_Id", cs.Class_Id),
                new SqlParameter("@Subject_Id", cs.Subject_Id),
                new SqlParameter("@IsCore", cs.IsCore),
                new SqlParameter("@IsActive", cs.IsActive),
                new SqlParameter("@Modified_By", cs.Modified_By)
            };

            _db.ExecuteProcedureNonQuery("sp_ClassSubject_Update", parameters);
        }

        // =============================
        // DELETE
        // =============================
        public void DeleteClassSubject(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@ClassSubject_Id", id)
            };

            _db.ExecuteProcedureNonQuery("sp_ClassSubject_Delete", parameters);
        }

        // =============================
        // MAPPING
        // =============================
        private static ClassSubject Map(DataRow row)
        {
            return new ClassSubject
            {
                ClassSubject_Id = (int)row["ClassSubject_Id"],
                Class_Id = (int)row["Class_Id"],
                Subject_Id = (int)row["Subject_Id"],
                SubjectName = row["Subject_Name"] != DBNull.Value ? (string)row["Subject_Name"] : string.Empty,
                IsCore = row["IsCore"] != DBNull.Value && (bool)row["IsCore"],
                IsActive = row["IsActive"] != DBNull.Value && (bool)row["IsActive"],
                IsDeleted = row["IsDeleted"] != DBNull.Value && (bool)row["IsDeleted"],
                Created_By = row["Created_By"] != DBNull.Value ? (string)row["Created_By"] : string.Empty,
                Created_Date = row["Created_Date"] != DBNull.Value ? (DateTime)row["Created_Date"] : DateTime.MinValue,
                Modified_By = row["Modified_By"] != DBNull.Value ? (string)row["Modified_By"] : string.Empty,
                Modified_Date = row["Modified_Date"] != DBNull.Value ? (DateTime)row["Modified_Date"] : DateTime.MinValue
            };
        }
    }
}
