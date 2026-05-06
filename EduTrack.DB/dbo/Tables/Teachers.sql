CREATE TABLE [dbo].[Teachers] (
    [Teacher_Id]    INT           IDENTITY (1, 1) NOT NULL,
    [User_Id]       INT           NOT NULL,
    [FullName]      VARCHAR (100) NOT NULL,
    [Created_By]    VARCHAR (50)  NOT NULL,
    [Created_Date]  DATETIME      DEFAULT (getdate()) NOT NULL,
    [Modified_By]   VARCHAR (50)  NOT NULL,
    [Modified_Date] DATETIME      DEFAULT (getdate()) NOT NULL,
    [isActive]      BIT           DEFAULT ((0)) NOT NULL,
    [isDeleted]     BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Teacher_Id] ASC),
    CONSTRAINT [FK_Teachers_User] FOREIGN KEY ([User_Id]) REFERENCES [dbo].[Users] ([User_Id]) ON DELETE CASCADE
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Teachers_User_Active]
    ON [dbo].[Teachers]([User_Id] ASC) WHERE ([isDeleted]=(0));

