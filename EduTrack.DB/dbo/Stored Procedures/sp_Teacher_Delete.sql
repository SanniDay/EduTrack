
/* =========================================================
   TEACHER DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_Teacher_Delete]
(
    @Teacher_Id INT
)
AS
BEGIN
    DELETE FROM Teachers
    WHERE Teacher_Id = @Teacher_Id
END