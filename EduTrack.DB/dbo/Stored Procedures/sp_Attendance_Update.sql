CREATE PROCEDURE sp_Attendance_Update
    @Attendance_Id INT,
    @Student_Class_Id INT,
    @ClassSubject_Id INT,
    @Date DATE,
    @Status VARCHAR(10),
    @Teacher INT,
    @Modified_By NVARCHAR(100)
AS
UPDATE Attendance
SET Student_Class_Id = @Student_Class_Id,
    ClassSubject_Id = @ClassSubject_Id,
    Attendance_Date = @Date,
    Status = @Status,
    Marked_By_Teacher_Id = @Teacher,
    Modified_By = @Modified_By,
    Modified_Date = GETDATE()
WHERE Attendance_Id = @Attendance_Id
