CREATE PROCEDURE [dbo].[sp_Attendance_Create]
    @Student_Class_Id INT,
    @ClassSubject_Id INT,
    @Date DATE,
    @Status VARCHAR(10),
    @Teacher INT,
    @IsActive BIT = 1,
    @Created_By NVARCHAR(100)
AS
BEGIN
    INSERT INTO Attendance(Student_Class_Id, ClassSubject_Id, Attendance_Date, Status, Marked_By_Teacher_Id, IsActive, IsDeleted, Created_By, Created_Date, Modified_By, Modified_Date)
    VALUES(@Student_Class_Id, @ClassSubject_Id, @Date, @Status, @Teacher, @IsActive, 0, @Created_By, GETDATE(), @Created_By, GETDATE());
END