CREATE PROCEDURE [dbo].[sp_TeacherClass_Update]
(
    @Teacher_Class_Id INT,
    @Teacher_Id INT,
    @ClassSubject_Id INT,
    @isActive BIT,
    @Modified_By VARCHAR(50)
)
AS
BEGIN
    UPDATE TeacherClass
    SET Teacher_Id = @Teacher_Id,
        ClassSubject_Id = @ClassSubject_Id,
        isActive = @isActive,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE Teacher_Class_Id = @Teacher_Class_Id
END