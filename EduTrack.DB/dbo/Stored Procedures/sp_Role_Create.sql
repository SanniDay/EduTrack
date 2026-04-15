CREATE PROCEDURE dbo.sp_Role_Create
    @Role_Name   VARCHAR(50),
    @Created_By  VARCHAR(50),
    @Modified_By VARCHAR(50) = NULL,
    @isActive    BIT = 0,
    @isDeleted   BIT = 0
AS
BEGIN
    SET NOCOUNT ON;

    -- Ensure Modified_By has a value
    SET @Modified_By = ISNULL(@Modified_By, @Created_By);

    INSERT INTO dbo.Roles
    (
        Role_Name,
        Created_By,
        Created_Date,
        Modified_By,
        Modified_Date,
        isActive,
        isDeleted
    )
    VALUES
    (
        @Role_Name,
        @Created_By,
        GETDATE(),
        @Modified_By,
        GETDATE(),
        @isActive,
        @isDeleted
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS Role_Id;
END;
