
-- =====================================================
-- TEACHERCLASS STORED PROCEDURES - VERIFIED (No changes needed, but included for reference)
-- =====================================================

CREATE PROCEDURE [dbo].[sp_TeacherClass_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT tc.*, t.FullName AS TeacherName, c.ClassName, s.Subject_Name
    FROM TeacherClass tc
    JOIN Teachers t ON t.Teacher_Id = tc.Teacher_Id
    LEFT JOIN ClassSubject cs ON cs.ClassSubject_Id = tc.ClassSubject_Id
    LEFT JOIN Classes c ON c.Class_Id = cs.Class_Id
    LEFT JOIN Subjects s ON s.Subject_Id = cs.Subject_Id
    WHERE tc.isDeleted = 0;
END;