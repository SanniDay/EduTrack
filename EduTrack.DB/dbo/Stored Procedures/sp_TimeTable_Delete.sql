
/* =========================================================
   TIMETABLE DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_Delete]
    @TimeTable_Id INT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    DELETE FROM TimeTable
    WHERE TimeTable_Id = @TimeTable_Id
END