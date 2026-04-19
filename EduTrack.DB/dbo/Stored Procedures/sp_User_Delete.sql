CREATE PROCEDURE [dbo].[sp_User_Delete]
    @User_Id INT,
    @Modified_By VARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE Users
        SET
            isDeleted = 1,
            isActive = 0,
            Modified_By = @Modified_By,
            Modified_Date = GETDATE()
        WHERE User_Id = @User_Id
          AND isDeleted = 0;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END
