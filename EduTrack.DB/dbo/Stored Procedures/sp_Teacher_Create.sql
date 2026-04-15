CREATE PROCEDURE sp_Teacher_Create
(
    @User_Id INT,
    @FullName VARCHAR(100),
    @Created_By VARCHAR(50)
)
AS
BEGIN
    INSERT INTO Teachers
    (
        User_Id,
        FullName,
        Created_By,
        Modified_By,
        Created_Date,
        Modified_Date,
        isActive,
        isDeleted
    )
    VALUES
    (
        @User_Id,
        @FullName,
        @Created_By,
        @Created_By,
        GETDATE(),
        GETDATE(),
        1,
        0
    )
END

