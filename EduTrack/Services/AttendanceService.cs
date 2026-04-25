using System.Data;
using Microsoft.Data.SqlClient;
using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;

namespace EduTrack.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly DbHelper _db;

        public AttendanceService(DbHelper db)
        {
            _db = db;
        }

        // =============================
        // GET ALL
        // =============================
        public List<Attendance> GetAllAttendance()
        {
            DataTable dt = _db.ExecuteProcedure("sp_Attendance_GetAll");

            List<Attendance> list = new();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(Map(row));
            }

            return list;
        }

        // =============================
        // GET BY ID
        // =============================
        public Attendance? GetAttendanceById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Attendance_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_Attendance_GetById", parameters);

            if (dt.Rows.Count == 0)
                return null;

            return Map(dt.Rows[0]);
        }

        // =============================
        // CREATE
        // =============================
        public void CreateAttendance(Attendance a)
        {
            var parameters = new[]
            {
                new SqlParameter("@Student_Class_Id", a.Student_Class_Id),
                new SqlParameter("@ClassSubject_Id", a.ClassSubject_Id.HasValue ? (object)a.ClassSubject_Id : DBNull.Value),
                new SqlParameter("@Date", a.Attendance_Date),
                new SqlParameter("@Status", a.Status),
                new SqlParameter("@Teacher", a.Marked_By_Teacher_Id.HasValue ? (object)a.Marked_By_Teacher_Id : DBNull.Value),
                new SqlParameter("@IsActive", a.IsActive),
                new SqlParameter("@Created_By", a.Created_By)
            };

            _db.ExecuteProcedureNonQuery("sp_Attendance_Create", parameters);
        }

        // =============================
        // UPDATE
        // =============================
        public void UpdateAttendance(Attendance a)
        {
            var parameters = new[]
            {
                new SqlParameter("@Attendance_Id", a.Attendance_Id),
                new SqlParameter("@Student_Class_Id", a.Student_Class_Id),
                new SqlParameter("@ClassSubject_Id", a.ClassSubject_Id.HasValue ? (object)a.ClassSubject_Id : DBNull.Value),
                new SqlParameter("@Date", a.Attendance_Date),
                new SqlParameter("@Status", a.Status),
                new SqlParameter("@Teacher", a.Marked_By_Teacher_Id.HasValue ? (object)a.Marked_By_Teacher_Id : DBNull.Value),
                new SqlParameter("@IsActive", a.IsActive),
                new SqlParameter("@Modified_By", a.Modified_By)
            };

            _db.ExecuteProcedureNonQuery("sp_Attendance_Update", parameters);
        }

        // =============================
        // DELETE
        // =============================
        public void DeleteAttendance(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@Attendance_Id", id)
            };

            _db.ExecuteProcedureNonQuery("sp_Attendance_Delete", parameters);
        }

        // =============================
        // MAPPING
        // =============================
        private static Attendance Map(DataRow row)
        {
            return new Attendance
            {
                Attendance_Id = (int)row["Attendance_Id"],
                Student_Class_Id = (int)row["Student_Class_Id"],
                ClassSubject_Id = row["ClassSubject_Id"] != DBNull.Value ? (int)row["ClassSubject_Id"] : null,
                Attendance_Date = (DateTime)row["Attendance_Date"],
                Status = row["Status"] != DBNull.Value ? (string)row["Status"] : string.Empty,
                Marked_By_Teacher_Id = row["Marked_By_Teacher_Id"] != DBNull.Value ? (int)row["Marked_By_Teacher_Id"] : null,
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
