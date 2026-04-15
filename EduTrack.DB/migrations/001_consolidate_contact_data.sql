-- ============================================================
-- Migration: Consolidate Phone_No and Address to Users table
-- Date: 2026-04-12
-- Description: 
--   1. Add Address column to Users table
--   2. Drop Phone_No from Teachers table
--   3. Drop Phone_No and Address from Students table
--   4. Update all affected stored procedures
-- ============================================================

-- =====================
-- STEP 1: Add Address to Users
-- =====================
IF NOT EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Users' AND COLUMN_NAME = 'Address'
)
BEGIN
    ALTER TABLE Users ADD [Address] VARCHAR(255) NULL;
    PRINT 'Added Address column to Users table.';
END
GO

-- =====================
-- STEP 2: Drop Phone_No from Teachers
-- =====================
IF EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Teachers' AND COLUMN_NAME = 'Phone_No'
)
BEGIN
    ALTER TABLE Teachers DROP COLUMN [Phone_No];
    PRINT 'Dropped Phone_No from Teachers table.';
END
GO

-- =====================
-- STEP 3: Drop Phone_No and Address from Students
-- =====================
IF EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Students' AND COLUMN_NAME = 'Phone_No'
)
BEGIN
    ALTER TABLE Students DROP COLUMN [Phone_No];
    PRINT 'Dropped Phone_No from Students table.';
END
GO

IF EXISTS (
    SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_NAME = 'Students' AND COLUMN_NAME = 'Address'
)
BEGIN
    ALTER TABLE Students DROP COLUMN [Address];
    PRINT 'Dropped Address from Students table.';
END
GO

-- =====================
-- STEP 4: Update sp_User_GetAll
-- =====================
ALTER PROCEDURE [dbo].[sp_User_GetAll]
AS
BEGIN
    SET NOCOUNT ON;
    SELECT	
        [User_Id],[User_Name],[PasswordHash],[Email],[Phone_Number],[Address],U.[Role_Id],R.[Role_Name],U.[Created_By],
        U.[Created_Date],U.[Modified_By],U.[Modified_Date],U.[isActive],U.[isDeleted] 
    FROM Users U
    LEFT JOIN Roles R ON R.Role_Id=U.Role_Id
    WHERE U.isDeleted=0
END
GO

-- =====================
-- STEP 5: Update sp_User_GetById
-- =====================
ALTER PROCEDURE [dbo].[sp_User_GetById]
    @User_Id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT	
        [User_Id],[User_Name],[PasswordHash],[Email],[Phone_Number],[Address],U.[Role_Id], R.[Role_Name], U.[Created_By],
        U.[Created_Date],U.[Modified_By],U.[Modified_Date],U.[isActive],U.[isDeleted] 
    FROM Users U
    LEFT JOIN Roles R ON R.Role_Id=U.Role_Id
    WHERE U.isDeleted=0 AND [User_Id]=@User_Id
END
GO

-- =====================
-- STEP 6: Update sp_User_Create
-- =====================
ALTER PROCEDURE [dbo].[sp_User_Create]
    @User_Name VARCHAR(50),
    @PasswordHash VARCHAR(225),
    @Email VARCHAR(50),
    @PhoneNumber VARCHAR(50),
    @Address VARCHAR(255) = NULL,
    @Role_Id INT = NULL,   
    @Created_By VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF EXISTS (SELECT 1 FROM Users WHERE User_Name = @User_Name AND isDeleted = 0)
        BEGIN
            RAISERROR('Username already exists.', 16, 1)
            RETURN
        END
        IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email AND isDeleted = 0)
        BEGIN
            RAISERROR('Email already exists.', 16, 1)
            RETURN
        END
        INSERT INTO Users (User_Name, PasswordHash, Email, Phone_Number, Address, Role_Id, Created_By, Created_Date, Modified_By, Modified_Date, isActive, isDeleted)
        VALUES (@User_Name, @PasswordHash, @Email, @PhoneNumber, @Address, @Role_Id, @Created_By, GETDATE(), @Created_By, GETDATE(), 0, 0);
        SELECT SCOPE_IDENTITY() AS NewUserId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO

-- =====================
-- STEP 7: Update sp_User_Update
-- =====================
ALTER PROCEDURE [dbo].[sp_User_Update]
    @User_Id INT,
    @User_Name VARCHAR(50),
    @Email VARCHAR(50),
    @Phone_Number VARCHAR(50),
    @Address VARCHAR(255) = NULL,
    @Role_Id INT,
    @Password VARCHAR(255),
    @Modified_By VARCHAR(50),
    @isActive BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        DECLARE @RoleName VARCHAR(50)
        SELECT TOP 1 @RoleName = Role_Name FROM Roles WHERE Role_Id = @Role_Id AND isDeleted = 0

        UPDATE Users
        SET User_Name = @User_Name, Email = @Email, Phone_Number = @Phone_Number, Address = @Address,
            Role_Id = @Role_Id, PasswordHash = @Password, isActive = @isActive,
            Modified_By = @Modified_By, Modified_Date = GETDATE()
        WHERE User_Id = @User_Id AND isDeleted = 0

        IF @RoleName = 'Student'
        BEGIN
            DELETE FROM Teachers WHERE User_Id = @User_Id
            MERGE Students AS target
            USING (SELECT @User_Id AS User_Id) AS source ON target.User_Id = source.User_Id
            WHEN MATCHED THEN
                UPDATE SET Modified_By = @Modified_By, Modified_Date = GETDATE()
            WHEN NOT MATCHED THEN
                INSERT (User_Id, FullName, Created_By, Created_Date, Modified_By, Modified_Date, isActive, isDeleted)
                VALUES (@User_Id, @User_Name, @Modified_By, GETDATE(), @Modified_By, GETDATE(), 1, 0);
        END

        IF @RoleName = 'Teacher'
        BEGIN
            DELETE FROM Students WHERE User_Id = @User_Id
            MERGE Teachers AS target
            USING (SELECT @User_Id AS User_Id) AS source ON target.User_Id = source.User_Id
            WHEN MATCHED THEN
                UPDATE SET Modified_By = @Modified_By, Modified_Date = GETDATE()
            WHEN NOT MATCHED THEN
                INSERT (User_Id, FullName, Created_By, Created_Date, Modified_By, Modified_Date, isActive, isDeleted)
                VALUES (@User_Id, @User_Name, @Modified_By, GETDATE(), @Modified_By, GETDATE(), 1, 0);
        END
    END TRY
    BEGIN CATCH
        THROW
    END CATCH
END
GO

-- =====================
-- STEP 8: Update sp_Teacher_GetAll (JOIN Users for Phone)
-- =====================
ALTER PROCEDURE [dbo].[sp_Teacher_GetAll]
AS
BEGIN
    SELECT T.*, U.Phone_Number AS Phone_No
    FROM Teachers T
    JOIN Users U ON T.User_Id = U.User_Id
    WHERE T.isDeleted = 0
END
GO

-- =====================
-- STEP 9: Update sp_Teacher_GetById (JOIN Users for Phone)
-- =====================
ALTER PROCEDURE [dbo].[sp_Teacher_GetById]
    @Teacher_Id INT
AS
BEGIN
    SELECT T.*, U.Phone_Number AS Phone_No
    FROM Teachers T
    JOIN Users U ON T.User_Id = U.User_Id
    WHERE T.Teacher_Id = @Teacher_Id
END
GO

-- =====================
-- STEP 10: Update sp_Teacher_Update (remove Phone_No)
-- =====================
ALTER PROCEDURE [dbo].[sp_Teacher_Update]
    @Teacher_Id INT,
    @User_Id INT,
    @FullName VARCHAR(100),
    @Created_By VARCHAR(50),
    @Modified_By VARCHAR(50),
    @IsActive BIT,
    @IsDeleted BIT
AS
BEGIN
    UPDATE Teachers
    SET User_Id = @User_Id, FullName = @FullName, Created_By = @Created_By,
        Modified_By = @Modified_By, IsActive = @IsActive, IsDeleted = @IsDeleted, Modified_Date = GETDATE()
    WHERE Teacher_Id = @Teacher_Id
END
GO

-- =====================
-- STEP 11: Update sp_Teacher_Insert (remove Phone_No)
-- =====================
ALTER PROCEDURE [dbo].[sp_Teacher_Insert]
    @User_Id INT,
    @FullName VARCHAR(100),
    @Created_By VARCHAR(50),
    @Modified_By VARCHAR(50),
    @IsActive BIT,
    @IsDeleted BIT
AS
BEGIN
    INSERT INTO Teachers (User_Id, FullName, Created_By, Modified_By, IsActive, IsDeleted)
    VALUES (@User_Id, @FullName, @Created_By, @Modified_By, @IsActive, @IsDeleted)
    SELECT SCOPE_IDENTITY()
END
GO

-- =====================
-- STEP 12: Update sp_Student_GetAll (JOIN Users for Phone & Address)
-- =====================
ALTER PROCEDURE [dbo].[sp_Student_GetAll]
AS
BEGIN
    SELECT S.*, U.Phone_Number AS Phone_No, U.Address AS Address
    FROM Students S
    JOIN Users U ON S.User_Id = U.User_Id
    WHERE S.isDeleted = 0
END
GO

-- =====================
-- STEP 13: Update sp_Student_GetById (JOIN Users for Phone & Address)
-- =====================
ALTER PROCEDURE [dbo].[sp_Student_GetById]
    @Student_Id INT
AS
BEGIN
    SELECT S.*, U.Phone_Number AS Phone_No, U.Address AS Address
    FROM Students S
    JOIN Users U ON S.User_Id = U.User_Id
    WHERE S.Student_Id = @Student_Id
END
GO

-- =====================
-- STEP 14: Update sp_Student_Update (remove Phone_No, Address)
-- =====================
ALTER PROCEDURE [dbo].[sp_Student_Update]
    @Student_Id INT,
    @FullName VARCHAR(100),
    @DOB DATE,
    @Gender VARCHAR(10),
    @Modified_By VARCHAR(50),
    @isActive BIT
AS
BEGIN
    UPDATE Students
    SET FullName = @FullName, DOB = @DOB, Gender = @Gender,
        Modified_By = @Modified_By, isActive = @isActive, Modified_Date = GETDATE()
    WHERE Student_Id = @Student_Id
END
GO

-- =====================
-- STEP 15: Update sp_Student_Insert (remove Phone_No, Address)
-- =====================
ALTER PROCEDURE [dbo].[sp_Student_Insert]
    @User_Id INT,
    @FullName VARCHAR(100),
    @DOB DATE,
    @Gender VARCHAR(10),
    @Created_By VARCHAR(50),
    @Modified_By VARCHAR(50),
    @IsActive BIT,
    @IsDeleted BIT
AS
BEGIN
    INSERT INTO Students (User_Id, FullName, DOB, Gender, Created_By, Modified_By, IsActive, IsDeleted)
    VALUES (@User_Id, @FullName, @DOB, @Gender, @Created_By, @Modified_By, @IsActive, @IsDeleted)
    SELECT SCOPE_IDENTITY()
END
GO

PRINT '===== Migration Complete: Contact data consolidated to Users table ====='
