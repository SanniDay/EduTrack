CREATE TABLE [dbo].[TeacherClass] (
    [Teacher_Class_Id] INT          IDENTITY (1, 1) NOT NULL,
    [Teacher_Id]       INT          NOT NULL,
    [Class_Id]         INT          NOT NULL,
    [Created_By]       VARCHAR (50) NOT NULL,
    [Created_Date]     DATETIME     DEFAULT (getdate()) NOT NULL,
    [Modified_By]      VARCHAR (50) NOT NULL,
    [Modified_Date]    DATETIME     DEFAULT (getdate()) NOT NULL,
    [isActive]         BIT          DEFAULT ((1)) NOT NULL,
    [isDeleted]        BIT          DEFAULT ((0)) NOT NULL,
    [ClassSubject_Id]  INT          NULL,
    PRIMARY KEY CLUSTERED ([Teacher_Class_Id] ASC),
    CONSTRAINT [FK_TeacherClass_CS] FOREIGN KEY ([ClassSubject_Id]) REFERENCES [dbo].[ClassSubject] ([ClassSubject_Id])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_TeacherClass]
    ON [dbo].[TeacherClass]([Teacher_Id] ASC, [ClassSubject_Id] ASC) WHERE ([isDeleted]=(0));

