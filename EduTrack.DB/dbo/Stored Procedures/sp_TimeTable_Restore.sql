

/* =========================================================
   PROCEDURE : RESTORE
   VERY USEFUL FOR SOFT DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_Restore]

    @TimeTable_Id INT,
    @Modified_By NVARCHAR(100)

AS
BEGIN

    SET NOCOUNT ON

    UPDATE TimeTable

    SET

        IsDeleted = 0,

        IsActive = 1,

        Modified_By = @Modified_By,

        Modified_Date = GETDATE()

    WHERE

        TimeTable_Id = @TimeTable_Id

END