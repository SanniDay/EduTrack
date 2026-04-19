CREATE PROCEDURE sp_TeacherClass_Delete
(
    @Teacher_Class_Id INT
)
AS
BEGIN

UPDATE TeacherClass
SET
    isDeleted = 1,
    isActive = 0
WHERE Teacher_Class_Id = @Teacher_Class_Id

END