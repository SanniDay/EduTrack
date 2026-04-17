CREATE TABLE [dbo].[Classes] (
    [Class_Id]      INT            IDENTITY (1, 1) NOT NULL,
    [ClassName]     NVARCHAR (100) NOT NULL,
    [Section]       NVARCHAR (50)  NOT NULL,
    [IsActive]      BIT            DEFAULT ((1)) NOT NULL,
    [IsDeleted]     BIT            DEFAULT ((0)) NOT NULL,
    [Created_By]    NVARCHAR (100) NOT NULL,
    [Created_Date]  DATETIME       DEFAULT (getdate()) NOT NULL,
    [Modified_By]   NVARCHAR (100) NULL,
    [Modified_Date] DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Class_Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Classes_IsDeleted]
    ON [dbo].[Classes]([IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Classes_IsActive]
    ON [dbo].[Classes]([IsActive] ASC);

