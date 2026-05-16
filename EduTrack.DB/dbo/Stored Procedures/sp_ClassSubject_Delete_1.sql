
/* =========================================================
   CLASS SUBJECT DELETE
========================================================= */

CREATE PROCEDURE [dbo].[sp_ClassSubject_Delete]
    @ClassSubject_Id INT
AS
BEGIN
    DELETE FROM ClassSubject
    WHERE ClassSubject_Id = @ClassSubject_Id
END