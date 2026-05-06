CREATE TABLE [dbo].[Students] (
    [Student_Id]    INT           IDENTITY (1, 1) NOT NULL,
    [User_Id]       INT           NOT NULL,
    [FullName]      VARCHAR (100) NOT NULL,
    [DOB]           DATE          NULL,
    [Gender]        VARCHAR (10)  NULL,
    [Created_By]    VARCHAR (50)  NOT NULL,
    [Created_Date]  DATETIME      DEFAULT (getdate()) NOT NULL,
    [Modified_By]   VARCHAR (50)  NOT NULL,
    [Modified_Date] DATETIME      DEFAULT (getdate()) NOT NULL,
    [isActive]      BIT           DEFAULT ((0)) NOT NULL,
    [isDeleted]     BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Student_Id] ASC),
    CONSTRAINT [FK_Students_User] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([User_Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Students_User_Active]
    ON [dbo].[Students]([User_Id] ASC) WHERE ([isDeleted]=(0));

