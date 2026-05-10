

/* =========================================================
   SUBJECTS
========================================================= */

CREATE PROCEDURE [dbo].[sp_Subject_PermanentDelete]
(
    @Subject_Id INT
)
AS
BEGIN

    DELETE FROM Subjects
    WHERE Subject_Id = @Subject_Id

END