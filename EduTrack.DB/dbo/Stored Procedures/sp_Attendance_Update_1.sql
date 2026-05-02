CREATE PROCEDURE [dbo].[sp_Attendance_Update]
    @Attendance_Id INT,
    @Student_Class_Id INT,
    @ClassSubject_Id INT,
    @Date DATE,
    @Status VARCHAR(10),
    @Teacher INT,
    @IsActive BIT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    UPDATE Attendance
    SET Student_Class_Id = @Student_Class_Id,
        ClassSubject_Id = @ClassSubject_Id,
        Attendance_Date = @Date,
        Status = @Status,
        Marked_By_Teacher_Id = @Teacher,
        IsActive = @IsActive,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE Attendance_Id = @Attendance_Id AND IsDeleted = 0;
END