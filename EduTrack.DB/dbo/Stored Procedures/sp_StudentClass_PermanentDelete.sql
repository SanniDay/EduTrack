

/* =========================================================
   STUDENT CLASS
========================================================= */

CREATE PROCEDURE [dbo].[sp_StudentClass_PermanentDelete]
(
    @Student_Class_Id INT
)
AS
BEGIN

    DELETE FROM StudentClass
    WHERE Student_Class_Id = @Student_Class_Id

END