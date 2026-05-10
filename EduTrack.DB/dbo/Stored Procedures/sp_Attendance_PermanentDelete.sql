/* =========================================================
   PERMANENT DELETE STORED PROCEDURES
   FOR ALL EXISTING TABLES
   HARD DELETE PROCEDURES
========================================================= */


/* =========================================================
   ATTENDANCE
========================================================= */

CREATE PROCEDURE [dbo].[sp_Attendance_PermanentDelete]
(
    @Attendance_Id INT
)
AS
BEGIN

    DELETE FROM Attendance
    WHERE Attendance_Id = @Attendance_Id

END