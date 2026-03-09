using System.Data;
using Microsoft.Data.SqlClient;

namespace EduTrack.Helpers
{
    public class DbHelper
    {
        private readonly string _connectionString;

        public DbHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                                ?? throw new Exception("Connection string not found.");
        }

        /// <summary>
        /// Creates and returns a new SQL connection.
        /// Used internally by all database execution methods.
        /// </summary>
        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        // ============================================================
        // INSERT / UPDATE / DELETE (No return value expected)
        // ============================================================
        //
        // Use this method when:
        // - You are only performing an action
        // - You only care about number of rows affected
        //
        // ⚠ NOTE:
        // If your stored procedure has SET NOCOUNT ON,
        // this will return -1. That is NORMAL behavior.
        //
        // Example:
        // - Update user
        // - Soft delete
        // - Insert where ID is NOT required
        //
        public int ExecuteProcedureNonQuery(string procedureName, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = GetConnection();
            using SqlCommand cmd = new SqlCommand(procedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            return cmd.ExecuteNonQuery(); // Returns rows affected (or -1 if NOCOUNT ON)
        }

        // ============================================================
        // RETURNING A SINGLE VALUE
        // ============================================================
        //
        // Use this method when:
        // - You expect ONLY ONE VALUE
        // - Identity value (SCOPE_IDENTITY)
        // - COUNT(*)
        // - EXISTS check
        // - Login validation
        //
        // Example:
        // SELECT SCOPE_IDENTITY()
        // SELECT COUNT(*)
        //
        public object? ExecuteProcedureScalar(string procedureName, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = GetConnection();
            using SqlCommand cmd = new SqlCommand(procedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            con.Open();
            return cmd.ExecuteScalar(); // Returns first column of first row
        }

        // ============================================================
        // RETURNING TABLE DATA (SELECT)
        // ============================================================
        //
        // Use this method when:
        // - You expect rows & columns
        // - Get user list
        // - Get by Id
        // - Search results
        //
        // Returns:
        // DataTable containing full result set
        //
        public DataTable ExecuteProcedure(string procedureName, SqlParameter[]? parameters = null)
        {
            using SqlConnection con = GetConnection();
            using SqlCommand cmd = new SqlCommand(procedureName, con);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
                cmd.Parameters.AddRange(parameters);

            using SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            return dt; // Full result set
        }
    }
}