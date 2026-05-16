
/* =========================================================
   STUDENT CLASS DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_StudentClass_Delete]
(
    @Student_Class_Id INT
)
AS
BEGIN
    DELETE FROM StudentClass
    WHERE Student_Class_Id = @Student_Class_Id
END