
/* =========================================================
   FEES DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_Fees_Delete]
    @Fees_Id INT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    DELETE FROM Fees
    WHERE Fees_Id = @Fees_Id
END