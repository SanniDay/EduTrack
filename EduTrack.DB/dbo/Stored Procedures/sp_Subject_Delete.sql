
/* =========================================================
   SUBJECT DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_Subject_Delete]
    @Subject_Id INT,
    @Modified_By NVARCHAR(100)
AS
BEGIN
    DELETE FROM Subjects
    WHERE Subject_Id = @Subject_Id
END