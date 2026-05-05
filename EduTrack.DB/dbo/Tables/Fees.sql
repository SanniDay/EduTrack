CREATE TABLE [dbo].[Fees] (
    [Fees_Id]       INT             IDENTITY (1, 1) NOT NULL,
    [Class_Id]      INT             NOT NULL,
    [FeeType]       INT             NOT NULL,
    [Amount]        DECIMAL (10, 2) NOT NULL,
    [Currency]      NVARCHAR (3)    DEFAULT ('USD') NOT NULL,
    [Description]   NVARCHAR (MAX)  NULL,
    [DueDate]       DATETIME        NULL,
    [IsActive]      BIT             DEFAULT ((1)) NOT NULL,
    [IsDeleted]     BIT             DEFAULT ((0)) NOT NULL,
    [Created_By]    NVARCHAR (100)  NOT NULL,
    [Created_Date]  DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified_By]   NVARCHAR (100)  NULL,
    [Modified_Date] DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Fees_Id] ASC),
    CONSTRAINT [FK_Fees_Class] FOREIGN KEY ([Class_Id]) REFERENCES [dbo].[Classes] ([Class_Id]) ON DELETE CASCADE,
    CONSTRAINT [UC_Fees_ClassType] UNIQUE NONCLUSTERED ([Class_Id] ASC, [FeeType] ASC)
);




GO
CREATE NONCLUSTERED INDEX [IX_Fees_IsDeleted]
    ON [dbo].[Fees]([IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Fees_FeeType]
    ON [dbo].[Fees]([FeeType] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Fees_ClassId]
    ON [dbo].[Fees]([Class_Id] ASC);

