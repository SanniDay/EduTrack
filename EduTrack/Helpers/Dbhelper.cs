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
            try
            {
                using SqlConnection con = GetConnection();
                using SqlCommand cmd = new SqlCommand(procedureName, con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteNonQuery(); // Returns rows affected (or -1 if NOCOUNT ON)
            }
            catch (SqlException ex)
            {
                throw MapToDataAccessException(ex, procedureName);
            }
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
            try
            {
                using SqlConnection con = GetConnection();
                using SqlCommand cmd = new SqlCommand(procedureName, con);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteScalar(); // Returns first column of first row
            }
            catch (SqlException ex)
            {
                throw MapToDataAccessException(ex, procedureName);
            }
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
            try
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
            catch (SqlException ex)
            {
                throw MapToDataAccessException(ex, procedureName);
            }
        }

        private Exception MapToDataAccessException(SqlException ex, string procedureName)
        {
            // Map common SQL errors to user friendly messages
            string friendly;

            switch (ex.Number)
            {
                case 2627: // Unique constraint error
                case 2601:
                    friendly = "A record with the same key already exists. Please check your input and try again.";
                    break;
                case 547: // Constraint check violation (FK)
                    friendly = "The operation failed because related data exists. Remove dependent records first or contact support.";
                    break;
                case 1205: // Deadlock
                    friendly = "The operation could not be completed because the database is busy. Please try again.";
                    break;
                case 8152: // String or binary data would be truncated
                    friendly = "One of the input values is too long. Please shorten the text and try again.";
                    break;
                default:
                    friendly = "A database error occurred. Please try again or contact support if the problem persists.";
                    break;
            }

            // Include procedure name for logging/debugging in inner exception, but do not expose raw DB details to users
            var message = $"{friendly}";

            // Return a custom exception so callers can inspect if needed
            return new DataAccessException(message, ex, procedureName);
        }
    }
}