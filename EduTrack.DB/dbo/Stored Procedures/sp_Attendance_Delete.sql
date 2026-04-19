CREATE PROCEDURE sp_Attendance_Delete
    @Attendance_Id INT
AS
UPDATE Attendance
SET IsDeleted = 1,
    Modified_Date = GETDATE()
WHERE Attendance_Id = @Attendance_Id
