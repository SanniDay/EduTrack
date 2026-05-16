
/* =========================================================
   STUDENT FEES DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_StudentFees_Delete]
    @StudentFees_Id INT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    DELETE FROM StudentFees
    WHERE StudentFees_Id = @StudentFees_Id
END