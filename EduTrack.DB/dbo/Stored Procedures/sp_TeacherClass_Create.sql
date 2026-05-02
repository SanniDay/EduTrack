CREATE PROCEDURE [dbo].[sp_TeacherClass_Create]
    @Teacher_Id INT,
    @ClassSubject_Id INT,
    @isActive BIT = 1,
    @Created_By VARCHAR(50)
AS
BEGIN
    INSERT INTO TeacherClass(Teacher_Id, Class_Id, ClassSubject_Id, Created_By, Created_Date, Modified_By, Modified_Date, isActive, isDeleted)
    SELECT @Teacher_Id, Class_Id, @ClassSubject_Id, @Created_By, GETDATE(), @Created_By, GETDATE(), @isActive, 0
    FROM ClassSubject WHERE ClassSubject_Id = @ClassSubject_Id;
END