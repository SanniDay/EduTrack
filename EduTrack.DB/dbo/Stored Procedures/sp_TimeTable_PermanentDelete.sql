

/* =========================================================
   TIMETABLE
========================================================= */

CREATE PROCEDURE [dbo].[sp_TimeTable_PermanentDelete]
(
    @TimeTable_Id INT
)
AS
BEGIN

    DELETE FROM TimeTable
    WHERE TimeTable_Id = @TimeTable_Id

END