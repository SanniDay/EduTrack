CREATE PROCEDURE dbo.sp_Role_Delete
    @Role_Id     INT,
    @Modified_By VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Soft delete: set isDeleted = 1 and isActive = 0
    UPDATE dbo.Roles
    SET
        isDeleted = 1,
        isActive = 0,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE Role_Id = @Role_Id
      AND isDeleted = 0;
END;
