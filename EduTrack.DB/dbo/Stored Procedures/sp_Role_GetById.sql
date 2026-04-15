CREATE PROCEDURE dbo.sp_Role_GetById
    @Role_Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        Role_Id,
        Role_Name,
        Created_By,
        Created_Date,
        Modified_By,
        Modified_Date,
        isActive,
        isDeleted
    FROM dbo.Roles
    WHERE Role_Id = @Role_Id
      AND isDeleted = 0;
END;
