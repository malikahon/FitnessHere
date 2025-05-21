-- triggers

-- business constraint (5 transactions limit per day for each member)
CREATE OR ALTER TRIGGER trigger_Transactions_FivePlus
ON Transactions
AFTER INSERT, UPDATE
AS
BEGIN
    -- declaring local variables 
    DECLARE @MemberID INT;
    DECLARE @TransactionDate DATE;
    SELECT @MemberID = MemberID, @TransactionDate = CAST(TransactionDate AS DATE) FROM INSERTED;
    IF @MemberID IS NOT NULL
    BEGIN
        -- business constraint (select all from transactions, where the same member has more than 5 transactions on the same date)
        IF (SELECT COUNT(*) FROM Transactions 
            WHERE MemberID = @MemberID 
            AND CAST(TransactionDate AS DATE) = @TransactionDate) > 5
        BEGIN
            ROLLBACK TRANSACTION;
            THROW 50001, 'A single member cannot have more than 5 transactions per day.', 1;
        END
    END
END;
GO

--custom access control (no data modifications 23-9)
CREATE OR ALTER TRIGGER trigger_Transactions_OffTime 
ON Transactions
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
  -- declare local variables 
  DECLARE @CurrentTime TIME = CONVERT(TIME, GETDATE());
  -- if the time is 23-9, throw error
  IF @CurrentTime >= '23:00' OR @CurrentTime < '09:00'
  BEGIN
    ROLLBACK TRANSACTION;
    THROW 51000, 'Your ass is not allowed to update your system between 11pm-9am. Please also give your system a break.', 1; 
  END
END;
GO

-- data logging trigger
-- if members log already exists, pls delete
IF OBJECT_ID('dbo.Members_Log', 'U') IS NOT NULL
    DROP TABLE dbo.Members_Log;
-- create Members_Log + logging columns
SELECT *, GETDATE() AS OperationDate, '' AS Operation, ORIGINAL_LOGIN() AS OperationUser 
INTO Members_Log 
FROM Members 
WHERE 1<>1;
GO
-- checking it was created
SELECT * FROM Members_Log;
-- default constraint
IF NOT EXISTS (
    SELECT 1 FROM sys.default_constraints WHERE parent_object_id = OBJECT_ID('dbo.Members_Log') AND name = 'DF_OperationDate'
)
BEGIN
    ALTER TABLE Members_Log 
    ADD CONSTRAINT DF_OperationDate DEFAULT GETDATE() FOR OperationDate;
END;
GO
-- logging trigger nitty gritty
CREATE OR ALTER TRIGGER trigger_Members_Log_Changes
ON Members
AFTER INSERT, UPDATE, DELETE 
AS
BEGIN
    -- logging delete
    INSERT INTO Members_Log (
        FirstName, LastName, PhoneNumber, DateOfBirth, Email, 
        RegistrationDate, ProfilePicture, IsDisabled, IsMale, 
        Operation, OperationDate, OperationUser
    ) 
    SELECT 
        FirstName, LastName, PhoneNumber, DateOfBirth, Email, 
        RegistrationDate, ProfilePicture, IsDisabled, IsMale, 
        'D', GETDATE(), ORIGINAL_LOGIN()
    FROM DELETED;
    -- logging insert 
    INSERT INTO Members_Log (
        FirstName, LastName, PhoneNumber, DateOfBirth, Email, 
        RegistrationDate, ProfilePicture, IsDisabled, IsMale, 
        Operation, OperationDate, OperationUser
    ) 
    SELECT 
        FirstName, LastName, PhoneNumber, DateOfBirth, Email, 
        RegistrationDate, ProfilePicture, IsDisabled, IsMale, 
        'I', GETDATE(), ORIGINAL_LOGIN()
    FROM INSERTED;
    -- logging update 
    INSERT INTO Members_Log (
        FirstName, LastName, PhoneNumber, DateOfBirth, Email, 
        RegistrationDate, ProfilePicture, IsDisabled, IsMale, 
        Operation, OperationDate, OperationUser
    ) 
    SELECT 
        FirstName, LastName, PhoneNumber, DateOfBirth, Email, 
        RegistrationDate, ProfilePicture, IsDisabled, IsMale, 
        'U', GETDATE(), ORIGINAL_LOGIN()
    FROM INSERTED
    WHERE EXISTS (SELECT 1 FROM DELETED WHERE DELETED.MemberID = INSERTED.MemberID);
END;
GO