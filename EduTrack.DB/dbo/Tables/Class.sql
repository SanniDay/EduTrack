CREATE TABLE [dbo].[Class] (
    [Class_Id]      INT          IDENTITY (1, 1) NOT NULL,
    [ClassName]     VARCHAR (50) NOT NULL,
    [Section]       VARCHAR (10) NOT NULL,
    [Created_By]    VARCHAR (50) NOT NULL,
    [Created_Date]  DATETIME     DEFAULT (getdate()) NOT NULL,
    [Modified_By]   VARCHAR (50) NOT NULL,
    [Modified_Date] DATETIME     DEFAULT (getdate()) NOT NULL,
    [isActive]      BIT          DEFAULT ((1)) NOT NULL,
    [isDeleted]     BIT          DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Class_Id] ASC)
);

