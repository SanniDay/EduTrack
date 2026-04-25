CREATE PROCEDURE [dbo].[sp_User_Create]
    @User_Name VARCHAR(50),
    @PasswordHash VARCHAR(225),
    @Email VARCHAR(50),
    @PhoneNumber VARCHAR(50),
    @Address VARCHAR(255) = '',
    @Role_Id INT = NULL,   
    @Created_By VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        -- Check duplicate username
        IF EXISTS (SELECT 1 FROM Users WHERE User_Name = @User_Name AND isDeleted = 0)
        BEGIN
            RAISERROR('Username already exists.', 16, 1)
            RETURN
        END

        -- Check duplicate email
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND isDeleted = 0)
        BEGIN
            RAISERROR('Email already exists.', 16, 1)
            RETURN
        END

        INSERT INTO Users
        (
            User_Name,
            PasswordHash,
            Email,
            Phone_Number,
            Address,
            Role_Id,
            Created_By,
            Created_Date,
            Modified_By,
            Modified_Date,
            isActive,
            isDeleted
        )
        VALUES
        (
            @User_Name,
            @PasswordHash,
            @Email,
            @PhoneNumber,
            @Address,
            @Role_Id,     
            @Created_By,
            GETDATE(),
            @Created_By,
            GETDATE(),
            0,
            0
        );

        SELECT SCOPE_IDENTITY() AS NewUserId;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END