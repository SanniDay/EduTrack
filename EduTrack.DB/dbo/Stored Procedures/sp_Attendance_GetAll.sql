
CREATE   PROCEDURE sp_Attendance_GetAll
AS
SELECT a.*,s.FullName,c.ClassName,sub.Subject_Name
FROM Attendance a
JOIN StudentClass sc ON sc.Student_Class_Id=a.Student_Class_Id
JOIN Students s ON s.Student_Id=sc.Student_Id
LEFT JOIN ClassSubject cs ON cs.ClassSubject_Id=a.ClassSubject_Id
LEFT JOIN Classes c ON c.Class_Id=cs.Class_Id
LEFT JOIN Subjects sub ON sub.Subject_Id=cs.Subject_Id
WHERE a.IsDeleted=0;