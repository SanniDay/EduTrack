CREATE PROCEDURE sp_TeacherClass_Update
(
    @Teacher_Class_Id INT,
    @Teacher_Id INT,
    @Class_Id INT,
    @Subject VARCHAR(100),
    @Modified_By VARCHAR(50)
)
AS
BEGIN

UPDATE TeacherClass
SET
    Teacher_Id = @Teacher_Id,
    Class_Id = @Class_Id,
    Subject = @Subject,
    Modified_By = @Modified_By,
    Modified_Date = GETDATE()
WHERE Teacher_Class_Id = @Teacher_Class_Id

END