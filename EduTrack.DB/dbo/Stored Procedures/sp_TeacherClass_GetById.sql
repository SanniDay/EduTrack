CREATE PROCEDURE sp_TeacherClass_GetById
(
    @Teacher_Class_Id INT
)
AS
BEGIN

SELECT *
FROM TeacherClass
WHERE Teacher_Class_Id = @Teacher_Class_Id
AND isDeleted = 0

END