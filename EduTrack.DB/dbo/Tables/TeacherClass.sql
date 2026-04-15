CREATE TABLE [dbo].[TeacherClass] (
    [Teacher_Class_Id] INT           IDENTITY (1, 1) NOT NULL,
    [Teacher_Id]       INT           NOT NULL,
    [Class_Id]         INT           NOT NULL,
    [Subject]          VARCHAR (100) NOT NULL,
    [Created_By]       VARCHAR (50)  NOT NULL,
    [Created_Date]     DATETIME      DEFAULT (getdate()) NOT NULL,
    [Modified_By]      VARCHAR (50)  NOT NULL,
    [Modified_Date]    DATETIME      DEFAULT (getdate()) NOT NULL,
    [isActive]         BIT           DEFAULT ((1)) NOT NULL,
    [isDeleted]        BIT           DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Teacher_Class_Id] ASC)
);

