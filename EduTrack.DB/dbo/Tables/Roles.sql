CREATE TABLE [dbo].[Roles] (
    [Role_Id]       INT          IDENTITY (1, 1) NOT NULL,
    [Role_Name]     VARCHAR (50) NOT NULL,
    [Created_By]    VARCHAR (50) NOT NULL,
    [Created_Date]  DATETIME     DEFAULT (getdate()) NULL,
    [Modified_By]   VARCHAR (50) NOT NULL,
    [Modified_Date] DATETIME     DEFAULT (getdate()) NULL,
    [isActive]      BIT          DEFAULT ((0)) NULL,
    [isDeleted]     BIT          DEFAULT ((0)) NULL,
    PRIMARY KEY CLUSTERED ([Role_Id] ASC)
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_Roles_Name_Active]
    ON [dbo].[Roles]([Role_Name] ASC) WHERE ([isDeleted]=(0));

