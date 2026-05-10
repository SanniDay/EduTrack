

/* =========================================================
   CLASSES
========================================================= */

CREATE PROCEDURE [dbo].[sp_Class_PermanentDelete]
(
    @Class_Id INT
)
AS
BEGIN

    DELETE FROM Classes
    WHERE Class_Id = @Class_Id

END