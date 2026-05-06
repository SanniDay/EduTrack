CREATE TABLE [dbo].[ClassSubject] (
    [ClassSubject_Id] INT            IDENTITY (1, 1) NOT NULL,
    [Class_Id]        INT            NOT NULL,
    [Subject_Id]      INT            NOT NULL,
    [IsCore]          BIT            DEFAULT ((1)) NULL,
    [IsActive]        BIT            DEFAULT ((1)) NULL,
    [IsDeleted]       BIT            DEFAULT ((0)) NULL,
    [Created_By]      NVARCHAR (100) NULL,
    [Created_Date]    DATETIME       DEFAULT (getdate()) NULL,
    [Modified_By]     NVARCHAR (100) NULL,
    [Modified_Date]   DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([ClassSubject_Id] ASC),
    CONSTRAINT [FK_ClassSubject_Class] FOREIGN KEY ([Class_Id]) REFERENCES [dbo].[Classes] ([Class_Id]),
    CONSTRAINT [FK_ClassSubject_Subject] FOREIGN KEY ([Subject_Id]) REFERENCES [dbo].[Subjects] ([Subject_Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ClassSubject]
    ON [dbo].[ClassSubject]([Class_Id] ASC, [Subject_Id] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_ClassSubject_Active]
    ON [dbo].[ClassSubject]([Class_Id] ASC, [Subject_Id] ASC) WHERE ([IsDeleted]=(0));

