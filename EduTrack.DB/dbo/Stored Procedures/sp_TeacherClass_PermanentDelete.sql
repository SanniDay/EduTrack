

/* =========================================================
   TEACHER CLASS
========================================================= */

CREATE PROCEDURE [dbo].[sp_TeacherClass_PermanentDelete]
(
    @Teacher_Class_Id INT
)
AS
BEGIN

    DELETE FROM TeacherClass
    WHERE Teacher_Class_Id = @Teacher_Class_Id

END