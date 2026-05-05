using EduTrack.Interfaces;
using EduTrack.Models;
using EduTrack.Helpers;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Services
{
    public class StudentFeesService : IStudentFeesService
    {
        private readonly DbHelper _db;

        public StudentFeesService(DbHelper db)
        {
            _db = db;
        }

        public List<StudentFees> GetAll()
        {
            DataTable dt = _db.ExecuteProcedure("sp_StudentFees_GetAll");
            return ToList(dt);
        }

        public StudentFees? GetById(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@StudentFees_Id", id)
            };

            DataTable dt = _db.ExecuteProcedure("sp_StudentFees_GetById", parameters);

            if (dt.Rows.Count == 0) return null;

            return Map(dt.Rows[0]);
        }

        public List<StudentFees> GetByStudentId(int studentId)
        {
            var parameters = new[]
            {
                new SqlParameter("@Student_Id", studentId)
            };

            DataTable dt = _db.ExecuteProcedure("sp_StudentFees_GetByStudentId", parameters);
            return ToList(dt);
        }

        public List<StudentFees> GetByFeesId(int feesId)
        {
            var parameters = new[]
            {
                new SqlParameter("@Fees_Id", feesId)
            };

            DataTable dt = _db.ExecuteProcedure("sp_StudentFees_GetByFeesId", parameters);
            return ToList(dt);
        }

        public void Create(StudentFees studentFees)
        {
            var parameters = new[]
            {
                new SqlParameter("@Student_Id", studentFees.Student_Id),
                new SqlParameter("@Fees_Id", studentFees.Fees_Id),
                new SqlParameter("@Amount", studentFees.Amount),
                new SqlParameter("@PaymentStatus", (int)studentFees.Status),
                new SqlParameter("@DueDate", studentFees.DueDate),
                new SqlParameter("@PaidDate", studentFees.PaidDate),
                new SqlParameter("@PaymentMethod", string.IsNullOrWhiteSpace(studentFees.PaymentMethod) ? (object)DBNull.Value : studentFees.PaymentMethod),
                new SqlParameter("@Notes", string.IsNullOrWhiteSpace(studentFees.Notes) ? (object)DBNull.Value : studentFees.Notes),
                new SqlParameter("@Created_By", studentFees.Created_By)
            };

            // sp_StudentFees_Create returns inserted identity (SCOPE_IDENTITY())
            var result = _db.ExecuteProcedureScalar("sp_StudentFees_Create", parameters);
            if (result != null && int.TryParse(result.ToString(), out int newId))
            {
                studentFees.StudentFees_Id = newId;
            }
        }

        public void Update(StudentFees studentFees)
        {
            var parameters = new[]
            {
                new SqlParameter("@StudentFees_Id", studentFees.StudentFees_Id),
                new SqlParameter("@DueDate", studentFees.DueDate),
                new SqlParameter("@Amount", studentFees.Amount),
                new SqlParameter("@PaymentStatus", (int)studentFees.Status),
                new SqlParameter("@PaymentMethod", studentFees.PaymentMethod),
                new SqlParameter("@Receipt_No", studentFees.Receipt_No),
                new SqlParameter("@Notes", studentFees.Notes),
                new SqlParameter("@Modified_By", studentFees.Modified_By)
            };

            _db.ExecuteProcedureNonQuery("sp_StudentFees_Update", parameters);
        }

        public void UpdateStatus(int studentFeesId, PaymentStatus status, DateTime? paidDate = null)
        {
            var parameters = new[]
            {
                new SqlParameter("@StudentFees_Id", studentFeesId),
                new SqlParameter("@PaymentStatus", (int)status),
                new SqlParameter("@PaidDate", (object?)paidDate ?? DBNull.Value),
                new SqlParameter("@Modified_By", "System")
            };

            _db.ExecuteProcedureNonQuery("sp_StudentFees_UpdateStatus", parameters);
        }

        public void Delete(int id)
        {
            var parameters = new[]
            {
                new SqlParameter("@StudentFees_Id", id),
                new SqlParameter("@Modified_By", "System")
            };

            _db.ExecuteProcedureNonQuery("sp_StudentFees_Delete", parameters);
        }

        private List<StudentFees> ToList(DataTable dt)
        {
            List<StudentFees> list = new();

            foreach (DataRow row in dt.Rows)
                list.Add(Map(row));

            return list;
        }

        private StudentFees Map(DataRow row)
        {
            return new StudentFees(
                Convert.ToInt32(row["StudentFees_Id"]),
                Convert.ToInt32(row["Student_Id"]),
                Convert.ToInt32(row["Fees_Id"]),
                row["DueDate"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["DueDate"],
                row["PaidDate"] == DBNull.Value ? null : (DateTime)row["PaidDate"],
                Convert.ToDecimal(row["Amount"]),
                (PaymentStatus)Convert.ToInt32(row["PaymentStatus"]),
                row["PaymentMethod"]?.ToString() ?? "",
                row["Receipt_No"]?.ToString() ?? "",
                row["Notes"]?.ToString() ?? "",
                row["Created_By"]?.ToString() ?? "",
                row["Created_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Created_Date"],
                row["Modified_By"]?.ToString() ?? "",
                row["Modified_Date"] == DBNull.Value ? DateTime.MinValue : (DateTime)row["Modified_Date"],
                row["IsDeleted"] == DBNull.Value ? false : (bool)row["IsDeleted"]
            );
        }
    }
}
