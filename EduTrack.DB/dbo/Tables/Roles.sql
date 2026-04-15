CREATE TABLE [dbo].[Roles] (
    [Role_Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Role_Name]     VARCHAR (50) NOT NULL,
    [Created_By]    VARCHAR (50) NOT NULL,
    [Created_Date]  DATETIME     DEFAULT (getdate()) NULL,
    [Modified_By]   VARCHAR (50) NOT NULL,
    [Modified_Date] DATETIME     DEFAULT (getdate()) NULL,
    [isActive]      BIT          DEFAULT ((0)) NULL,
    [isDeleted]     BIT          DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Role_Id] ASC),
    UNIQUE NONCLUSTERED ([Role_Name] ASC)
);

