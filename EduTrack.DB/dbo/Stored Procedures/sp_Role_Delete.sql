
/* =========================================================
   ROLE DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_Role_Delete]
    @Role_Id INT,
    @Modified_By VARCHAR(50)
AS
BEGIN
    DELETE FROM Roles
    WHERE Role_Id = @Role_Id
END