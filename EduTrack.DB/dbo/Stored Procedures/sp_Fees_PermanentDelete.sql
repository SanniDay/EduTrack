

/* =========================================================
   FEES
========================================================= */

CREATE PROCEDURE [dbo].[sp_Fees_PermanentDelete]
(
    @Fees_Id INT
)
AS
BEGIN

    DELETE FROM Fees
    WHERE Fees_Id = @Fees_Id

END