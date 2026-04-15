CREATE PROCEDURE sp_TeacherClass_GetAll
AS
BEGIN

SELECT *
FROM TeacherClass
WHERE isDeleted = 0

END