

/* =========================================================
   ROLES
========================================================= */

CREATE PROCEDURE [dbo].[sp_Role_PermanentDelete]
(
    @Role_Id INT
)
AS
BEGIN

    DELETE FROM Roles
    WHERE Role_Id = @Role_Id

END