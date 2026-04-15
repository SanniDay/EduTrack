CREATE PROCEDURE [dbo].[sp_User_Update]
    @User_Id INT,
    @User_Name VARCHAR(50),
    @Email VARCHAR(50),
    @Phone_Number VARCHAR(50),
    @Address VARCHAR(255),
    @Role_Id INT,
    @Password VARCHAR(255),
    @Modified_By VARCHAR(50),
    @isActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        DECLARE @RoleName VARCHAR(50)

        -- Get Role Name from Roles table
        SELECT TOP 1 @RoleName = Role_Name
        FROM Roles
        WHERE Role_Id = @Role_Id
        AND isDeleted = 0


        -----------------------
        -- Update Users
        -----------------------
        UPDATE Users
        SET
            User_Name = @User_Name,
            Email = @Email,
            Phone_Number = @Phone_Number,
            Address = @Address,
            Role_Id = @Role_Id,
            PasswordHash = @Password,
            isActive = @isActive,
            Modified_By = @Modified_By,
            Modified_Date = GETDATE()
        WHERE User_Id = @User_Id
        AND isDeleted = 0


        -----------------------
        -- If Role = Student
        -----------------------
        IF @RoleName = 'Student'
        BEGIN

            DELETE FROM Teachers
            WHERE User_Id = @User_Id

            MERGE Students AS target
            USING (SELECT @User_Id AS User_Id) AS source
            ON target.User_Id = source.User_Id

            WHEN MATCHED THEN
                UPDATE SET
                    Modified_By = @Modified_By,
                    Modified_Date = GETDATE()

            WHEN NOT MATCHED THEN
                INSERT
                (
                    User_Id,
                    FullName,
                    Created_By,
                    Created_Date,
                    Modified_By,
                    Modified_Date,
                    isActive,
                    isDeleted
                )
                VALUES
                (
                    @User_Id,
                    @User_Name,
                    @Modified_By,
                    GETDATE(),
                    @Modified_By,
                    GETDATE(),
                    1,
                    0
                );

        END


        -----------------------
        -- If Role = Teacher
        -----------------------
        IF @RoleName = 'Teacher'
        BEGIN

            DELETE FROM Students
            WHERE User_Id = @User_Id

            MERGE Teachers AS target
            USING (SELECT @User_Id AS User_Id) AS source
            ON target.User_Id = source.User_Id

            WHEN MATCHED THEN
                UPDATE SET
                    Modified_By = @Modified_By,
                    Modified_Date = GETDATE()

            WHEN NOT MATCHED THEN
                INSERT
                (
                    User_Id,
                    FullName,
                    Created_By,
                    Created_Date,
                    Modified_By,
                    Modified_Date,
                    isActive,
                    isDeleted
                )
                VALUES
                (
                    @User_Id,
                    @User_Name,
                    @Modified_By,
                    GETDATE(),
                    @Modified_By,
                    GETDATE(),
                    1,
                    0
                );

        END

    END TRY
    BEGIN CATCH
        THROW
    END CATCH
END