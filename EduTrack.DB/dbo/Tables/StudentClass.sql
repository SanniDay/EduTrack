CREATE TABLE [dbo].[StudentClass] (
    [Student_Class_Id] INT          IDENTITY (1, 1) NOT NULL,
    [Student_Id]       INT          NOT NULL,
    [Class_Id]         INT          NOT NULL,
    [Created_By]       VARCHAR (50) NOT NULL,
    [Created_Date]     DATETIME     DEFAULT (getdate()) NOT NULL,
    [Modified_By]      VARCHAR (50) NOT NULL,
    [Modified_Date]    DATETIME     DEFAULT (getdate()) NOT NULL,
    [isActive]         BIT          DEFAULT ((1)) NOT NULL,
    [isDeleted]        BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Student_Class_Id] ASC)
);

