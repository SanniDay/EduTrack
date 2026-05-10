

/* =========================================================
   STUDENTS
========================================================= */

CREATE PROCEDURE [dbo].[sp_Student_PermanentDelete]
(
    @Student_Id INT
)
AS
BEGIN

    DELETE FROM Students
    WHERE Student_Id = @Student_Id

END