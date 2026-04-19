
-- =====================
-- STEP 15: Update sp_Student_Insert (remove Phone_No, Address)
-- =====================
CREATE PROCEDURE [dbo].[sp_Student_Create]
    @User_Id INT,
    @FullName VARCHAR(100),
    @DOB DATE,
    @Gender VARCHAR(10),
    @Created_By VARCHAR(50),
    @Modified_By VARCHAR(50),
    @IsActive BIT,
    @IsDeleted BIT
AS
BEGIN
    INSERT INTO Students (User_Id, FullName, DOB, Gender, Created_By, Modified_By, IsActive, IsDeleted)
    VALUES (@User_Id, @FullName, @DOB, @Gender, @Created_By, @Modified_By, @IsActive, @IsDeleted)
    SELECT SCOPE_IDENTITY()
END