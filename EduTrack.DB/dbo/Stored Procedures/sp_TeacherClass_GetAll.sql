CREATE PROCEDURE sp_TeacherClass_GetAll
AS
BEGIN

SELECT tc.*, c.ClassName, s.Subject_Name
FROM TeacherClass tc
LEFT JOIN ClassSubject cs ON cs.ClassSubject_Id = tc.ClassSubject_Id
LEFT JOIN Classes c ON c.Class_Id = cs.Class_Id
LEFT JOIN Subjects s ON s.Subject_Id = cs.Subject_Id
WHERE tc.isDeleted = 0

END
