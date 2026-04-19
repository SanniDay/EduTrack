CREATE PROCEDURE dbo.sp_Role_Update
    @Role_Id     INT,
    @Role_Name   VARCHAR(50),
    @Modified_By VARCHAR(50),
    @isActive    BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Roles
    SET
        Role_Name = @Role_Name,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE(),
        isActive = @isActive
    WHERE Role_Id = @Role_Id
      AND isDeleted = 0;
END;
