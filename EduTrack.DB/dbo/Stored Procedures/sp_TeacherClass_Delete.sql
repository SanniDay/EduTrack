
/* =========================================================
   TEACHER CLASS DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_TeacherClass_Delete]
(
    @Teacher_Class_Id INT
)
AS
BEGIN
    DELETE FROM TeacherClass
    WHERE Teacher_Class_Id = @Teacher_Class_Id
END