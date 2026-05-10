

/* =========================================================
   USERS
========================================================= */

CREATE PROCEDURE [dbo].[sp_User_PermanentDelete]
(
    @User_Id INT
)
AS
BEGIN

    DELETE FROM Users
    WHERE User_Id = @User_Id

END