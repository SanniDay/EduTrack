CREATE TABLE [dbo].[TimeTable] (
    [TimeTable_Id]    INT            IDENTITY (1, 1) NOT NULL,
    [Class_Id]        INT            NOT NULL,
    [ClassSubject_Id] INT            NOT NULL,
    [Teacher_Id]      INT            NOT NULL,
    [Day_Name]        VARCHAR (20)   NOT NULL,
    [Period_No]       INT            NOT NULL,
    [Start_Time]      TIME (7)       NOT NULL,
    [End_Time]        TIME (7)       NOT NULL,
    [Room_No]         NVARCHAR (50)  NULL,
    [IsActive]        BIT            CONSTRAINT [DF_TimeTable_IsActive] DEFAULT ((1)) NOT NULL,
    [IsDeleted]       BIT            CONSTRAINT [DF_TimeTable_IsDeleted] DEFAULT ((0)) NOT NULL,
    [Created_By]      NVARCHAR (100) NOT NULL,
    [Created_Date]    DATETIME       CONSTRAINT [DF_TimeTable_CreatedDate] DEFAULT (getdate()) NOT NULL,
    [Modified_By]     NVARCHAR (100) NULL,
    [Modified_Date]   DATETIME       NULL,
    CONSTRAINT [PK_TimeTable] PRIMARY KEY CLUSTERED ([TimeTable_Id] ASC),
    CONSTRAINT [CHK_TimeTable_Day] CHECK ([Day_Name]='Saturday' OR [Day_Name]='Friday' OR [Day_Name]='Thursday' OR [Day_Name]='Wednesday' OR [Day_Name]='Tuesday' OR [Day_Name]='Monday'),
    CONSTRAINT [FK_TimeTable_Class] FOREIGN KEY ([Class_Id]) REFERENCES [dbo].[Classes] ([Class_Id]),
    CONSTRAINT [FK_TimeTable_ClassSubject] FOREIGN KEY ([ClassSubject_Id]) REFERENCES [dbo].[ClassSubject] ([ClassSubject_Id]),
    CONSTRAINT [FK_TimeTable_Teacher] FOREIGN KEY ([Teacher_Id]) REFERENCES [dbo].[Teachers] ([Teacher_Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_TimeTable_IsDeleted]
    ON [dbo].[TimeTable]([IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TimeTable_Day]
    ON [dbo].[TimeTable]([Day_Name] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TimeTable_Teacher]
    ON [dbo].[TimeTable]([Teacher_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TimeTable_Class]
    ON [dbo].[TimeTable]([Class_Id] ASC);

