using EduTrack.Helpers;
using EduTrack.Interfaces;
using EduTrack.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace EduTrack.Services
{
    public class TimeTableService : ITimeTableService
    {
        private readonly DbHelper _db;

        public TimeTableService(DbHelper db)
        {
            _db = db;
        }

        public List<TimeTable> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_TimeTable_GetAll");
            return ToList(dt);
        }

        public TimeTable? GetById(int id)
        {
            var param = new[] { new SqlParameter("@TimeTable_Id", id) };
            DataTable dt = _db.ExecuteProcedure("sp_TimeTable_GetById", param);
            if (dt.Rows.Count == 0) return null;
            return Map(dt.Rows[0]);
        }

        public List<TimeTable> GetByClass(int classId)
        {
            var param = new[] { new SqlParameter("@Class_Id", classId) };
            DataTable dt = _db.ExecuteProcedure("sp_TimeTable_GetByClass", param);
            return ToList(dt);
        }

        public int Create(TimeTable t)
        {
            // Check duplicates: Class + Day_Name + Period_No
            var existing = GetByClass(t.Class_Id).FirstOrDefault(x => x.Day_Name == t.Day_Name && x.Period_No == t.Period_No && !x.IsDeleted);
            if (existing != null)
            {
                // Duplicate found
                return 0;
            }

            var parameters = new[]
            {
                new SqlParameter("@Class_Id", t.Class_Id),
                new SqlParameter("@ClassSubject_Id", (object?)t.ClassSubject_Id ?? DBNull.Value),
                new SqlParameter("@Teacher_Id", (object?)t.Teacher_Id ?? DBNull.Value),
                new SqlParameter("@Day_Name", t.Day_Name),
                new SqlParameter("@Period_No", t.Period_No),
                new SqlParameter("@Start_Time", t.Start_Time),
                new SqlParameter("@End_Time", t.End_Time),
                new SqlParameter("@Room_No", string.IsNullOrWhiteSpace(t.Room_No) ? (object)DBNull.Value : t.Room_No),
                new SqlParameter("@Created_By", t.Created_By ?? "System")
            };

            var result = _db.ExecuteProcedureScalar("sp_TimeTable_Create", parameters);
            if (result != null && int.TryParse(result.ToString(), out int newId))
            {
                return newId;
            }
            return 0;
        }

        public int Update(TimeTable t)
        {
            // Check duplicate excluding current
            var existing = GetByClass(t.Class_Id).FirstOrDefault(x => x.Day_Name == t.Day_Name && x.Period_No == t.Period_No && x.TimeTable_Id != t.TimeTable_Id && !x.IsDeleted);
            if (existing != null)
            {
                return 0;
            }

            var parameters = new[]
            {
                new SqlParameter("@TimeTable_Id", t.TimeTable_Id),
                new SqlParameter("@Class_Id", t.Class_Id),
                new SqlParameter("@ClassSubject_Id", (object?)t.ClassSubject_Id ?? DBNull.Value),
                new SqlParameter("@Teacher_Id", (object?)t.Teacher_Id ?? DBNull.Value),
                new SqlParameter("@Day_Name", t.Day_Name),
                new SqlParameter("@Period_No", t.Period_No),
                new SqlParameter("@Start_Time", t.Start_Time),
                new SqlParameter("@End_Time", t.End_Time),
                new SqlParameter("@Room_No", string.IsNullOrWhiteSpace(t.Room_No) ? (object)DBNull.Value : t.Room_No),
                new SqlParameter("@Modified_By", t.Modified_By ?? "System")
            };

            return _db.ExecuteProcedureNonQuery("sp_TimeTable_Update", parameters);
        }

        public int Delete(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@TimeTable_Id", id),
                new SqlParameter("@Modified_By", "System")
            };
            return _db.ExecuteProcedureNonQuery("sp_TimeTable_Delete", parameters);
        }

        public int Restore(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@TimeTable_Id", id),
                new SqlParameter("@Modified_By", "System")
            };
            return _db.ExecuteProcedureNonQuery("sp_TimeTable_Restore", parameters);
        }

        private List<TimeTable> ToList(DataTable dt)
        {
            var list = new List<TimeTable>();
            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));
            return list;
        }

        private TimeTable Map(DataRow row)
        {
            return new TimeTable
            {
                TimeTable_Id = row.Table.Columns.Contains("TimeTable_Id") && row["TimeTable_Id"] != DBNull.Value ? Convert.ToInt32(row["TimeTable_Id"]) : 0,
                Class_Id = row.Table.Columns.Contains("Class_Id") && row["Class_Id"] != DBNull.Value ? Convert.ToInt32(row["Class_Id"]) : 0,
                ClassSubject_Id = row.Table.Columns.Contains("ClassSubject_Id") && row["ClassSubject_Id"] != DBNull.Value ? (int?)Convert.ToInt32(row["ClassSubject_Id"]) : null,
                Teacher_Id = row.Table.Columns.Contains("Teacher_Id") && row["Teacher_Id"] != DBNull.Value ? (int?)Convert.ToInt32(row["Teacher_Id"]) : null,
                Day_Name = row.Table.Columns.Contains("Day_Name") ? row["Day_Name"]?.ToString() ?? string.Empty : string.Empty,
                Period_No = row.Table.Columns.Contains("Period_No") && row["Period_No"] != DBNull.Value ? Convert.ToInt32(row["Period_No"]) : 0,
                Start_Time = row.Table.Columns.Contains("Start_Time") && row["Start_Time"] != DBNull.Value ? (TimeSpan)row["Start_Time"] : TimeSpan.Zero,
                End_Time = row.Table.Columns.Contains("End_Time") && row["End_Time"] != DBNull.Value ? (TimeSpan)row["End_Time"] : TimeSpan.Zero,
                Room_No = row.Table.Columns.Contains("Room_No") ? row["Room_No"]?.ToString() ?? string.Empty : string.Empty,
                IsActive = row.Table.Columns.Contains("IsActive") ? (row["IsActive"] == DBNull.Value ? false : (bool)row["IsActive"]) : false,
                IsDeleted = row.Table.Columns.Contains("IsDeleted") ? (row["IsDeleted"] == DBNull.Value ? false : (bool)row["IsDeleted"]) : false,
                Created_By = row.Table.Columns.Contains("Created_By") ? row["Created_By"]?.ToString() ?? string.Empty : string.Empty,
                Created_Date = row.Table.Columns.Contains("Created_Date") && row["Created_Date"] != DBNull.Value ? (DateTime)row["Created_Date"] : DateTime.MinValue,
                Modified_By = row.Table.Columns.Contains("Modified_By") ? row["Modified_By"]?.ToString() ?? string.Empty : string.Empty,
                Modified_Date = row.Table.Columns.Contains("Modified_Date") && row["Modified_Date"] != DBNull.Value ? (DateTime)row["Modified_Date"] : DateTime.MinValue
            };
        }
    }
}
