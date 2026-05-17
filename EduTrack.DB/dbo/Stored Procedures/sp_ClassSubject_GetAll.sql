
CREATE   PROCEDURE [dbo].[sp_ClassSubject_GetAll]
AS
SELECT cs.*,c.ClassName + ' - ' + c.Section AS ClassName,s.Subject_Name
FROM ClassSubject cs
JOIN Classes c ON c.Class_Id=cs.Class_Id
JOIN Subjects s ON s.Subject_Id=cs.Subject_Id
WHERE cs.IsDeleted=0;