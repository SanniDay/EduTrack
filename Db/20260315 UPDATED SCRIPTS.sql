USE [EduTrackdb]
GO
/****** Object:  StoredProcedure [dbo].[sp_Class_Create]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Class_Create]
(
    @ClassName VARCHAR(50),
    @Section VARCHAR(10),
    @Created_By VARCHAR(50)
)
AS
BEGIN

INSERT INTO Class
(
    ClassName,
    Section,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
)
VALUES
(
    @ClassName,
    @Section,
    @Created_By,
    GETDATE(),
    @Created_By,
    GETDATE(),
    1,
    0
)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Class_Delete]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Class_Delete]
(
    @Class_Id INT
)
AS
BEGIN

UPDATE Class
SET
    isDeleted = 1,
    isActive = 0
WHERE Class_Id = @Class_Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Class_GetAll]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Class_GetAll]
AS
BEGIN

SELECT
    Class_Id,
    ClassName,
    Section,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
FROM Class
WHERE isDeleted = 0

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Class_GetById]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Class_GetById]
(
    @Class_Id INT
)
AS
BEGIN

SELECT *
FROM Class
WHERE Class_Id = @Class_Id
AND isDeleted = 0

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Class_Update]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Class_Update]
(
    @Class_Id INT,
    @ClassName VARCHAR(50),
    @Section VARCHAR(10),
    @Modified_By VARCHAR(50)
)
AS
BEGIN

UPDATE Class
SET
    ClassName = @ClassName,
    Section = @Section,
    Modified_By = @Modified_By,
    Modified_Date = GETDATE()
WHERE Class_Id = @Class_Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Role_Create]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Role_Create]
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

GO
/****** Object:  StoredProcedure [dbo].[sp_Role_Delete]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Role_Delete]
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

GO
/****** Object:  StoredProcedure [dbo].[sp_Role_GetAll]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Role_GetAll]
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

GO
/****** Object:  StoredProcedure [dbo].[sp_Role_GetById]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Role_GetById]
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

GO
/****** Object:  StoredProcedure [dbo].[sp_Role_Update]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Role_Update]
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

GO
/****** Object:  StoredProcedure [dbo].[sp_Student_Create]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Student_Create]
(
    @User_Id INT,
    @FullName VARCHAR(100),
    @DOB DATE,
    @Gender VARCHAR(10),
    @Phone_No VARCHAR(15),
    @Address VARCHAR(255),
    @Created_By VARCHAR(50)
)
AS
BEGIN
    INSERT INTO Students
    (
        User_Id,
        FullName,
        DOB,
        Gender,
        Phone_No,
        Address,
        Created_By,
        Modified_By,
        Created_Date,
        Modified_Date,
        isActive,
        isDeleted
    )
    VALUES
    (
        @User_Id,
        @FullName,
        @DOB,
        @Gender,
        @Phone_No,
        @Address,
        @Created_By,
        @Created_By,
        GETDATE(),
        GETDATE(),
        1,
        0
    )
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Student_Delete]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Student_Delete]
(
    @Student_Id INT
)
AS
BEGIN
    UPDATE Students
    SET
        isDeleted = 1,
        isActive = 0,
        Modified_Date = GETDATE()
    WHERE Student_Id = @Student_Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Student_GetAll]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Student_GetAll]
AS
BEGIN
    SELECT *
    FROM Students
    WHERE isDeleted = 0
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Student_GetById]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Student_GetById]
(
    @Student_Id INT
)
AS
BEGIN
    SELECT *
    FROM Students
    WHERE Student_Id = @Student_Id
    AND isDeleted = 0
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Student_Update]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Student_Update]
(
    @Student_Id INT,
    @FullName VARCHAR(100),
    @DOB DATE,
    @Gender VARCHAR(10),
    @Phone_No VARCHAR(15),
    @Address VARCHAR(255),
    @Modified_By VARCHAR(50)
)
AS
BEGIN
    UPDATE Students
    SET
        FullName = @FullName,
        DOB = @DOB,
        Gender = @Gender,
        Phone_No = @Phone_No,
        Address = @Address,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE Student_Id = @Student_Id
END

GO
/****** Object:  StoredProcedure [dbo].[sp_StudentClass_Create]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_StudentClass_Create]
(
    @Student_Id INT,
    @Class_Id INT,
    @Created_By VARCHAR(50)
)
AS
BEGIN

INSERT INTO StudentClass
(
    Student_Id,
    Class_Id,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
)
VALUES
(
    @Student_Id,
    @Class_Id,
    @Created_By,
    GETDATE(),
    @Created_By,
    GETDATE(),
    1,
    0
)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_StudentClass_Delete]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_StudentClass_Delete]
(
    @Student_Class_Id INT
)
AS
BEGIN

UPDATE StudentClass
SET
    isDeleted = 1,
    isActive = 0
WHERE Student_Class_Id = @Student_Class_Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_StudentClass_GetAll]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_StudentClass_GetAll]
AS
BEGIN

SELECT *
FROM StudentClass
WHERE isDeleted = 0

END
GO
/****** Object:  StoredProcedure [dbo].[sp_StudentClass_GetById]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_StudentClass_GetById]
(
    @Student_Class_Id INT
)
AS
BEGIN

SELECT *
FROM StudentClass
WHERE Student_Class_Id = @Student_Class_Id
AND isDeleted = 0

END
GO
/****** Object:  StoredProcedure [dbo].[sp_StudentClass_Update]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_StudentClass_Update]
(
    @Student_Class_Id INT,
    @Student_Id INT,
    @Class_Id INT,
    @Modified_By VARCHAR(50)
)
AS
BEGIN

UPDATE StudentClass
SET
    Student_Id = @Student_Id,
    Class_Id = @Class_Id,
    Modified_By = @Modified_By,
    Modified_Date = GETDATE()
WHERE Student_Class_Id = @Student_Class_Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_Teacher_Create]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_Teacher_Create]
(
    @User_Id INT,
    @FullName VARCHAR(100),
    @Phone_No VARCHAR(15),
    @Created_By VARCHAR(50)
)
AS
BEGIN
    INSERT INTO Teachers
    (
        User_Id,
        FullName,
        Phone_No,
        Created_By,
        Modified_By,
        Created_Date,
        Modified_Date,
        isActive,
        isDeleted
    )
    VALUES
    (
        @User_Id,
        @FullName,
        @Phone_No,
        @Created_By,
        @Created_By,
        GETDATE(),
        GETDATE(),
        1,
        0
    )
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Teacher_Delete]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Teacher_Delete]
(
    @Teacher_Id INT
)
AS
BEGIN
    UPDATE Teachers
    SET
        isDeleted = 1,
        isActive = 0,
        Modified_Date = GETDATE()
    WHERE Teacher_Id = @Teacher_Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_Teacher_GetAll]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Teacher_GetAll]
AS
BEGIN
    SELECT *
    FROM Teachers
    WHERE isDeleted = 0
END

GO
/****** Object:  StoredProcedure [dbo].[sp_Teacher_GetById]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Teacher_GetById]
(
    @Teacher_Id INT
)
AS
BEGIN
    SELECT *
    FROM Teachers
    WHERE Teacher_Id = @Teacher_Id
    AND isDeleted = 0
END 

GO
/****** Object:  StoredProcedure [dbo].[sp_Teacher_Update]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_Teacher_Update]
(
    @Teacher_Id INT,
    @FullName VARCHAR(100),
    @Phone_No VARCHAR(15),
    @Modified_By VARCHAR(50)
)
AS
BEGIN
    UPDATE Teachers
    SET
        FullName = @FullName,
        Phone_No = @Phone_No,
        Modified_By = @Modified_By,
        Modified_Date = GETDATE()
    WHERE Teacher_Id = @Teacher_Id
END

GO
/****** Object:  StoredProcedure [dbo].[sp_TeacherClass_Create]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_TeacherClass_Create]
(
    @Teacher_Id INT,
    @Class_Id INT,
    @Subject VARCHAR(100),
    @Created_By VARCHAR(50)
)
AS
BEGIN

INSERT INTO TeacherClass
(
    Teacher_Id,
    Class_Id,
    Subject,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
)
VALUES
(
    @Teacher_Id,
    @Class_Id,
    @Subject,
    @Created_By,
    GETDATE(),
    @Created_By,
    GETDATE(),
    1,
    0
)

END
GO
/****** Object:  StoredProcedure [dbo].[sp_TeacherClass_Delete]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_TeacherClass_Delete]
(
    @Teacher_Class_Id INT
)
AS
BEGIN

UPDATE TeacherClass
SET
    isDeleted = 1,
    isActive = 0
WHERE Teacher_Class_Id = @Teacher_Class_Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_TeacherClass_GetAll]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_TeacherClass_GetAll]
AS
BEGIN

SELECT *
FROM TeacherClass
WHERE isDeleted = 0

END
GO
/****** Object:  StoredProcedure [dbo].[sp_TeacherClass_GetById]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_TeacherClass_GetById]
(
    @Teacher_Class_Id INT
)
AS
BEGIN

SELECT *
FROM TeacherClass
WHERE Teacher_Class_Id = @Teacher_Class_Id
AND isDeleted = 0

END
GO
/****** Object:  StoredProcedure [dbo].[sp_TeacherClass_Update]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_TeacherClass_Update]
(
    @Teacher_Class_Id INT,
    @Teacher_Id INT,
    @Class_Id INT,
    @Subject VARCHAR(100),
    @Modified_By VARCHAR(50)
)
AS
BEGIN

UPDATE TeacherClass
SET
    Teacher_Id = @Teacher_Id,
    Class_Id = @Class_Id,
    Subject = @Subject,
    Modified_By = @Modified_By,
    Modified_Date = GETDATE()
WHERE Teacher_Class_Id = @Teacher_Class_Id

END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Authenticate]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_User_Authenticate]
    @EmailOrUsername NVARCHAR(100),
    @PasswordHash NVARCHAR(256)
AS
BEGIN
    SELECT * 
    FROM Users
    WHERE 
        ([User_Name] = @EmailOrUsername OR Email = @EmailOrUsername)
        AND PasswordHash = @PasswordHash
        AND isDeleted = 0
        AND isActive = 1;  
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Create]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_User_Create]
    @User_Name VARCHAR(50),
    @PasswordHash VARCHAR(225),
    @Email VARCHAR(50),
    @PhoneNumber VARCHAR(50),
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
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Delete]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_User_Delete]
    @User_Id INT,
    @Modified_By VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE Users
        SET
            isDeleted = 1,
            isActive = 0,
            Modified_By = @Modified_By,
            Modified_Date = GETDATE()
        WHERE User_Id = @User_Id
          AND isDeleted = 0;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_GetAll]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE[dbo].[sp_User_GetAll]
	
AS
BEGIN

	SET NOCOUNT ON;
	SELECT	
			[User_Id],[User_Name],[PasswordHash],[Email],[Phone_Number],U.[Role_Id],R.[Role_Name],U.[Created_By],
			U.[Created_Date],U.[Modified_By],U.[Modified_Date],U.[isActive],U.[isDeleted] 
	FROM Users U
	LEFT JOIN Roles R	ON R.Role_Id=U.Role_Id
	WHERE U.isDeleted=0
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_GetById]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_User_GetById]
	@User_Id INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	
			[User_Id],[User_Name],[PasswordHash],[Email],[Phone_Number],U.[Role_Id], R.[Role_Name], U.[Created_By],
			U.[Created_Date],U.[Modified_By],U.[Modified_Date],U.[isActive],U.[isDeleted] 
	FROM Users U
	LEFT JOIN Roles R ON R.Role_Id=U.Role_Id
	WHERE U.isDeleted=0 AND [User_Id]=@User_Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Update]    Script Date: 15-03-2026 21:20:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_User_Update]
    @User_Id INT,
    @User_Name VARCHAR(50),
    @Email VARCHAR(50),
    @Phone_Number VARCHAR(50),
    @Role_Id INT,
    @Password VARCHAR(255), -- Added
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
                    Phone_No = @Phone_Number,
                    Modified_By = @Modified_By,
                    Modified_Date = GETDATE()

            WHEN NOT MATCHED THEN
                INSERT
                (
                    User_Id,
                    FullName,
                    Phone_No,
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
                    @Phone_Number,
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
                    Phone_No = @Phone_Number,
                    Modified_By = @Modified_By,
                    Modified_Date = GETDATE()

            WHEN NOT MATCHED THEN
                INSERT
                (
                    User_Id,
                    FullName,
                    Phone_No,
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
                    @Phone_Number,
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
GO
