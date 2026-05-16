
/* =========================================================
   STUDENT DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_Student_Delete]
(
    @Student_Id INT
)
AS
BEGIN
    DELETE FROM Students
    WHERE Student_Id = @Student_Id
END