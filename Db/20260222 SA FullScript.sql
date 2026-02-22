USE [master]
GO
/****** Object:  Database [EduTrackdb]    Script Date: 22-02-2026 20:29:17 ******/
CREATE DATABASE EduTrackDb
ON PRIMARY
(
    NAME = EduTrackDb_Data,
    SIZE = 10MB,
    FILEGROWTH = 10MB
)
LOG ON
(
    NAME = EduTrackDb_Log,
    SIZE = 5MB,
    FILEGROWTH = 5MB
);
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [EduTrackdb].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [EduTrackdb] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [EduTrackdb] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [EduTrackdb] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [EduTrackdb] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [EduTrackdb] SET ARITHABORT OFF 
GO
ALTER DATABASE [EduTrackdb] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [EduTrackdb] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [EduTrackdb] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [EduTrackdb] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [EduTrackdb] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [EduTrackdb] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [EduTrackdb] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [EduTrackdb] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [EduTrackdb] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [EduTrackdb] SET  ENABLE_BROKER 
GO
ALTER DATABASE [EduTrackdb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [EduTrackdb] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [EduTrackdb] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [EduTrackdb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [EduTrackdb] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [EduTrackdb] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [EduTrackdb] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [EduTrackdb] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [EduTrackdb] SET  MULTI_USER 
GO
ALTER DATABASE [EduTrackdb] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [EduTrackdb] SET DB_CHAINING OFF 
GO
ALTER DATABASE [EduTrackdb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [EduTrackdb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [EduTrackdb] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [EduTrackdb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [EduTrackdb] SET QUERY_STORE = OFF
GO
USE [EduTrackdb]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[Class_Id] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [varchar](50) NULL,
	[Section] [varchar](10) NULL,
	[Created_By] [varchar](50) NOT NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [varchar](50) NOT NULL,
	[Modified_Date] [datetime] NULL,
	[isActive] [bit] NULL,
	[isDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Class_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[Role_Id] [int] IDENTITY(1,1) NOT NULL,
	[Role_Name] [varchar](50) NOT NULL,
	[Created_By] [varchar](50) NOT NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [varchar](50) NOT NULL,
	[Modified_Date] [datetime] NULL,
	[isActive] [bit] NULL,
	[isDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Role_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Role_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Students]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Students](
	[Student_Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [int] NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[DOB] [date] NULL,
	[Gender] [varchar](10) NULL,
	[Phone_No] [varchar](15) NOT NULL,
	[Address] [varchar](255) NULL,
	[Created_By] [varchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [varchar](50) NOT NULL,
	[Modified_Date] [datetime] NOT NULL,
	[isActive] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Student_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Students__206D9171A4561648] UNIQUE NONCLUSTERED 
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Subjects]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Subjects](
	[Subject_Id] [int] IDENTITY(1,1) NOT NULL,
	[SubjectName] [varchar](50) NULL,
	[Created_By] [varchar](50) NOT NULL,
	[Created_Date] [datetime] NULL,
	[Modified_By] [varchar](50) NOT NULL,
	[Modified_Date] [datetime] NULL,
	[isActive] [bit] NULL,
	[isDeleted] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[Subject_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teachers]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teachers](
	[Teacher_Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Id] [int] NOT NULL,
	[FullName] [varchar](100) NOT NULL,
	[Phone_No] [varchar](15) NOT NULL,
	[Created_By] [varchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [varchar](50) NOT NULL,
	[Modified_Date] [datetime] NOT NULL,
	[isActive] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Teacher_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Teachers_User_Id] UNIQUE NONCLUSTERED 
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[User_Id] [int] IDENTITY(1,1) NOT NULL,
	[User_Name] [varchar](50) NOT NULL,
	[PasswordHash] [varchar](225) NOT NULL,
	[Email] [varchar](50) NOT NULL,
	[Phone_Number] [varchar](25) NOT NULL,
	[Role_Id] [int] NULL,
	[Created_By] [varchar](50) NOT NULL,
	[Created_Date] [datetime] NOT NULL,
	[Modified_By] [varchar](50) NOT NULL,
	[Modified_Date] [datetime] NOT NULL,
	[isActive] [bit] NOT NULL,
	[isDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[User_Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[User_Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Class] ADD  DEFAULT (getdate()) FOR [Created_Date]
GO
ALTER TABLE [dbo].[Class] ADD  DEFAULT (getdate()) FOR [Modified_Date]
GO
ALTER TABLE [dbo].[Class] ADD  DEFAULT ((0)) FOR [isActive]
GO
ALTER TABLE [dbo].[Class] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (getdate()) FOR [Created_Date]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT (getdate()) FOR [Modified_Date]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT ((0)) FOR [isActive]
GO
ALTER TABLE [dbo].[Roles] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT (getdate()) FOR [Created_Date]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT (getdate()) FOR [Modified_Date]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [isActive]
GO
ALTER TABLE [dbo].[Students] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[Subjects] ADD  DEFAULT (getdate()) FOR [Created_Date]
GO
ALTER TABLE [dbo].[Subjects] ADD  DEFAULT (getdate()) FOR [Modified_Date]
GO
ALTER TABLE [dbo].[Subjects] ADD  DEFAULT ((0)) FOR [isActive]
GO
ALTER TABLE [dbo].[Subjects] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT (getdate()) FOR [Created_Date]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT (getdate()) FOR [Modified_Date]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT ((0)) FOR [isActive]
GO
ALTER TABLE [dbo].[Teachers] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [Role_Id]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [Created_Date]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT (getdate()) FOR [Modified_Date]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [isActive]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((0)) FOR [isDeleted]
GO
ALTER TABLE [dbo].[Students]  WITH CHECK ADD  CONSTRAINT [FK_Students_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([User_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Students] CHECK CONSTRAINT [FK_Students_User]
GO
ALTER TABLE [dbo].[Teachers]  WITH CHECK ADD  CONSTRAINT [FK_Teachers_User] FOREIGN KEY([User_Id])
REFERENCES [dbo].[Users] ([User_Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Teachers] CHECK CONSTRAINT [FK_Teachers_User]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_Role] FOREIGN KEY([Role_Id])
REFERENCES [dbo].[Roles] ([Role_Id])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_Role]
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Authenticate]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_User_Authenticate]
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
/****** Object:  StoredProcedure [dbo].[sp_User_Create]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_User_Create]
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
/****** Object:  StoredProcedure [dbo].[sp_User_Delete]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_User_Delete]
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
/****** Object:  StoredProcedure [dbo].[sp_User_GetAll]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE[dbo].[sp_User_GetAll]
	
AS
BEGIN

	SET NOCOUNT ON;
	SELECT	
			[User_Id],[User_Name],[PasswordHash],[Email],[Phone_Number],[Role_Id],[Created_By],
			[Created_Date],[Modified_By],[Modified_Date],[isActive],[isDeleted] 
	FROM Users 
	WHERE isDeleted=0
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_GetById]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[sp_User_GetById]
	@User_Id INT
AS
BEGIN

	SET NOCOUNT ON;

	SELECT	
			[User_Id],[User_Name],[PasswordHash],[Email],[Phone_Number],[Role_Id],[Created_By],
			[Created_Date],[Modified_By],[Modified_Date],[isActive],[isDeleted] 
	FROM Users 
	WHERE isDeleted=0 AND [User_Id]=@User_Id
END
GO
/****** Object:  StoredProcedure [dbo].[sp_User_Update]    Script Date: 22-02-2026 20:29:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_User_Update]
    @User_Id INT,
    @User_Name VARCHAR(50),
    @Email VARCHAR(50),
    @Phone_Number  VARCHAR(50),
    @Role_Id INT,
    @Modified_By VARCHAR(50),
    @isActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE Users
        SET
            User_Name = @User_Name,
            Email = @Email,
            Phone_Number = @Phone_Number,
            Role_Id = @Role_Id,
            isActive = @isActive,
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
USE [master]
GO
ALTER DATABASE [EduTrackdb] SET  READ_WRITE 
GO
