CREATE PROCEDURE sp_TeacherClass_GetById
(
    @Teacher_Class_Id INT
)
AS
BEGIN

SELECT tc.*, c.ClassName, s.Subject_Name
FROM TeacherClass tc
LEFT JOIN ClassSubject cs ON cs.ClassSubject_Id = tc.ClassSubject_Id
LEFT JOIN Classes c ON c.Class_Id = cs.Class_Id
LEFT JOIN Subjects s ON s.Subject_Id = cs.Subject_Id
WHERE tc.Teacher_Class_Id = @Teacher_Class_Id
AND tc.isDeleted = 0

END
