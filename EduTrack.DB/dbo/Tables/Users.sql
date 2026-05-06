CREATE TABLE [dbo].[Users] (
    [User_Id]       INT           IDENTITY (1, 1) NOT NULL,
    [User_Name]     VARCHAR (50)  NOT NULL,
    [PasswordHash]  VARCHAR (225) NOT NULL,
    [Email]         VARCHAR (50)  NOT NULL,
    [Phone_Number]  VARCHAR (25)  NOT NULL,
    [Address]       VARCHAR (255) NULL,
    [Role_Id]       INT           DEFAULT ((0)) NULL,
    [Created_By]    VARCHAR (50)  NOT NULL,
    [Created_Date]  DATETIME      DEFAULT (getdate()) NOT NULL,
    [Modified_By]   VARCHAR (50)  NOT NULL,
    [Modified_Date] DATETIME      DEFAULT (getdate()) NOT NULL,
    [isActive]      BIT           DEFAULT ((0)) NOT NULL,
    [isDeleted]     BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([User_Id] ASC),
    CONSTRAINT [FK_Users_Role] FOREIGN KEY ([Role_Id]) REFERENCES [dbo].[Roles] ([Role_Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Users_UserName_Active]
    ON [dbo].[Users]([User_Name] ASC) WHERE ([isDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Users_Email_Active]
    ON [dbo].[Users]([Email] ASC) WHERE ([isDeleted]=(0));

