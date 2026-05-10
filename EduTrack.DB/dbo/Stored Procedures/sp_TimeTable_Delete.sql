

/* =========================================================
   PROCEDURE : DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_Delete]

    @TimeTable_Id INT,
    @Modified_By NVARCHAR(100)

AS
BEGIN

    SET NOCOUNT ON

    UPDATE TimeTable

    SET

        IsDeleted = 1,

        IsActive = 0,

        Modified_By = @Modified_By,

        Modified_Date = GETDATE()

    WHERE

        TimeTable_Id = @TimeTable_Id

        AND IsDeleted = 0

END