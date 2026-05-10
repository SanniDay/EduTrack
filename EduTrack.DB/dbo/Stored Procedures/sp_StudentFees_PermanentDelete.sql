

/* =========================================================
   STUDENT FEES
========================================================= */

CREATE PROCEDURE [dbo].[sp_StudentFees_PermanentDelete]
(
    @StudentFees_Id INT
)
AS
BEGIN

    DELETE FROM StudentFees
    WHERE StudentFees_Id = @StudentFees_Id

END