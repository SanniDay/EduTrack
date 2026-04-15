CREATE PROCEDURE sp_Class_Create
(
    @ClassName VARCHAR(50),
    @Section VARCHAR(10),
    @Created_By VARCHAR(50)
)
AS
BEGIN

INSERT INTO Class
(
    ClassName,
    Section,
    Created_By,
    Created_Date,
    Modified_By,
    Modified_Date,
    isActive,
    isDeleted
)
VALUES
(
    @ClassName,
    @Section,
    @Created_By,
    GETDATE(),
    @Created_By,
    GETDATE(),
    1,
    0
)

END