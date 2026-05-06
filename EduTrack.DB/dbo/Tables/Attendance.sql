CREATE TABLE [dbo].[Attendance] (
    [Attendance_Id]        INT            IDENTITY (1, 1) NOT NULL,
    [Student_Class_Id]     INT            NOT NULL,
    [ClassSubject_Id]      INT            NULL,
    [Attendance_Date]      DATE           NOT NULL,
    [Status]               VARCHAR (10)   NULL,
    [Marked_By_Teacher_Id] INT            NULL,
    [IsActive]             BIT            DEFAULT ((1)) NULL,
    [IsDeleted]            BIT            DEFAULT ((0)) NULL,
    [Created_By]           NVARCHAR (100) NULL,
    [Created_Date]         DATETIME       DEFAULT (getdate()) NULL,
    [Modified_By]          NVARCHAR (100) NULL,
    [Modified_Date]        DATETIME       NULL,
    PRIMARY KEY CLUSTERED ([Attendance_Id] ASC),
    CHECK ([Status]='Leave' OR [Status]='Late' OR [Status]='Absent' OR [Status]='Present'),
    CONSTRAINT [FK_Attendance_CS] FOREIGN KEY ([ClassSubject_Id]) REFERENCES [dbo].[ClassSubject] ([ClassSubject_Id]),
    CONSTRAINT [FK_Attendance_SC] FOREIGN KEY ([Student_Class_Id]) REFERENCES [dbo].[StudentClass] ([Student_Class_Id]),
    CONSTRAINT [FK_Attendance_T] FOREIGN KEY ([Marked_By_Teacher_Id]) REFERENCES [dbo].[Teachers] ([Teacher_Id])
);




GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Attendance]
    ON [dbo].[Attendance]([Student_Class_Id] ASC, [ClassSubject_Id] ASC, [Attendance_Date] ASC) WHERE ([IsDeleted]=(0));


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Attendance_Unique]
    ON [dbo].[Attendance]([Student_Class_Id] ASC, [Attendance_Date] ASC) WHERE ([IsDeleted]=(0));

