CREATE TABLE [dbo].[StudentFees] (
    [StudentFees_Id] INT             IDENTITY (1, 1) NOT NULL,
    [Student_Id]     INT             NOT NULL,
    [Fees_Id]        INT             NOT NULL,
    [Amount]         DECIMAL (10, 2) NOT NULL,
    [PaymentStatus]  INT             DEFAULT ((1)) NOT NULL,
    [DueDate]        DATETIME        NOT NULL,
    [PaidDate]       DATETIME        NULL,
    [PaymentMethod]  NVARCHAR (50)   NULL,
    [Receipt_No]     NVARCHAR (100)  NULL,
    [DiscountAmount] DECIMAL (10, 2) DEFAULT ((0)) NOT NULL,
    [FineAmount]     DECIMAL (10, 2) DEFAULT ((0)) NOT NULL,
    [TotalAmount]    DECIMAL (10, 2) NOT NULL,
    [Notes]          NVARCHAR (MAX)  NULL,
    [IsDeleted]      BIT             DEFAULT ((0)) NOT NULL,
    [Created_By]     NVARCHAR (100)  NOT NULL,
    [Created_Date]   DATETIME        DEFAULT (getdate()) NOT NULL,
    [Modified_By]    NVARCHAR (100)  NULL,
    [Modified_Date]  DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([StudentFees_Id] ASC),
    CONSTRAINT [FK_StudentFees_Fees] FOREIGN KEY ([Fees_Id]) REFERENCES [dbo].[Fees] ([Fees_Id]),
    CONSTRAINT [FK_StudentFees_Student] FOREIGN KEY ([Student_Id]) REFERENCES [dbo].[Students] ([Student_Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_StudentFees_IsDeleted]
    ON [dbo].[StudentFees]([IsDeleted] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StudentFees_DueDate]
    ON [dbo].[StudentFees]([DueDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StudentFees_PaymentStatus]
    ON [dbo].[StudentFees]([PaymentStatus] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StudentFees_FeesId]
    ON [dbo].[StudentFees]([Fees_Id] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StudentFees_StudentId]
    ON [dbo].[StudentFees]([Student_Id] ASC);


GO
CREATE UNIQUE NONCLUSTERED INDEX [UX_StudentFees_Unique_Active]
    ON [dbo].[StudentFees]([Student_Id] ASC, [Fees_Id] ASC) WHERE ([IsDeleted]=(0));

