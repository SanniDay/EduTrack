CREATE PROCEDURE sp_ClassSubject_GetById
    @ClassSubject_Id INT
AS
SELECT cs.*, c.ClassName, s.Subject_Name
FROM ClassSubject cs
JOIN Classes c ON c.Class_Id = cs.Class_Id
JOIN Subjects s ON s.Subject_Id = cs.Subject_Id
WHERE cs.ClassSubject_Id = @ClassSubject_Id
AND cs.IsDeleted = 0
