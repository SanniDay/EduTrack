CREATE TABLE [dbo].[Subjects] (
    [Subject_Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Subject_Name]  NVARCHAR (100) NOT NULL,
    [Subject_Code]  NVARCHAR (20)  NOT NULL,
    [Description]   NVARCHAR (MAX) NULL,
    [IsActive]      BIT            DEFAULT ((1)) NOT NULL,
    [IsDeleted]     BIT            DEFAULT ((0)) NOT NULL,
    [Created_By]    NVARCHAR (100) NOT NULL,
    [Created_Date]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified_By]   NVARCHAR (100) NULL,
    [Modified_Date] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Subject_Id] ASC),
    UNIQUE NONCLUSTERED ([Subject_Code] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Subjects_IsDeleted]
    ON [dbo].[Subjects]([IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Subjects_IsActive]
    ON [dbo].[Subjects]([IsActive] ASC);

