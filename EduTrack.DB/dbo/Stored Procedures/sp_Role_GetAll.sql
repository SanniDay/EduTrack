CREATE PROCEDURE dbo.sp_Role_GetAll
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
    WHERE isDeleted = 0
    ORDER BY Role_Name;
END;
