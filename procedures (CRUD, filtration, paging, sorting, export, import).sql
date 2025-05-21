-- CRUD
-- create (C)
CREATE OR ALTER PROCEDURE usp_Members_CreateMember
    @FirstName NVARCHAR(255),
    @LastName NVARCHAR(255),
    @PhoneNumber NVARCHAR(20),
    @DateOfBirth DATE,
    @Email NVARCHAR(255),
    @ProfilePicture VARBINARY(MAX),
    @IsDisabled BIT,
    @IsMale BIT
AS
BEGIN
  -- try catch
    BEGIN TRY
        INSERT INTO Members (FirstName, LastName, PhoneNumber, DateOfBirth, Email, RegistrationDate, ProfilePicture, IsDisabled, IsMale) 
        VALUES (@FirstName, @LastName, @PhoneNumber, @DateOfBirth, @Email, GETDATE(), @ProfilePicture, @IsDisabled, @IsMale);
    END TRY
    BEGIN CATCH
        -- error details
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        -- raising error
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH;
END;

GO

-- read (R)
CREATE OR ALTER PROCEDURE usp_Members_GetAllMembers
AS
BEGIN
  -- try catch
    BEGIN TRY
        SELECT 
            MemberID,
            FirstName,
            LastName,
            PhoneNumber,
            DateOfBirth,
            Email,
            RegistrationDate,
            ProfilePicture,
            IsDisabled,
            IsMale
        FROM Members;
    END TRY
    BEGIN CATCH
        -- error details
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        -- raising error
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH;
END;

GO
CREATE OR ALTER PROCEDURE usp_Members_GetMemberByID
    (@MemberID INT)
AS
BEGIN
  -- try catch
    BEGIN TRY
        SELECT 
            MemberID,
            FirstName,
            LastName,
            PhoneNumber,
            DateOfBirth,
            Email,
            RegistrationDate,
            ProfilePicture,
            IsDisabled,
            IsMale
        FROM Members
        WHERE MemberID = @MemberID;
    END TRY
    BEGIN CATCH
        -- error details
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        -- raising error
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH;
END;

GO




-- update (U)
CREATE OR ALTER PROCEDURE usp_Members_UpdateMember
(
    @MemberID INT,
    @FirstName NVARCHAR(255) = NULL,
    @LastName NVARCHAR(255) = NULL,
    @PhoneNumber NVARCHAR(20) = NULL,
    @DateOfBirth DATE = NULL,
    @Email NVARCHAR(255) = NULL,
    @ProfilePicture VARBINARY(MAX) = NULL,
    @IsDisabled BIT = NULL,
    @IsMale BIT = NULL
)
AS
BEGIN
  -- try catch
    BEGIN TRY
        UPDATE Members
        SET 
            FirstName = COALESCE(@FirstName, FirstName),
            LastName = COALESCE(@LastName, LastName),
            PhoneNumber = COALESCE(@PhoneNumber, PhoneNumber),
            DateOfBirth = COALESCE(@DateOfBirth, DateOfBirth),
            Email = COALESCE(@Email, Email),
            ProfilePicture = COALESCE(@ProfilePicture, ProfilePicture),
            IsDisabled = COALESCE(@IsDisabled, IsDisabled),
            IsMale = COALESCE(@IsMale, IsMale)
        WHERE MemberID = @MemberID;
        -- only if record updated
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR ('No member found with the given MemberID.', 16, 1);
            RETURN;
        END
    END TRY
    BEGIN CATCH
        -- error details
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        -- raising error
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH;
END;

GO






-- delete (D)
CREATE OR ALTER PROCEDURE usp_Members_DeleteMember
    (@MemberID INT)
AS
BEGIN
    BEGIN TRY
        DELETE FROM Members WHERE MemberID = @MemberID;
        -- only if record doesn't exist
        IF @@ROWCOUNT = 0
        BEGIN
            RAISERROR ('No member found with the given MemberID.', 16, 1);
            RETURN;
        END
    END TRY
    BEGIN CATCH
        -- error details
        DECLARE @ErrorMessage NVARCHAR(4000), @ErrorSeverity INT, @ErrorState INT;
        SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
        -- raising error
        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN;
    END CATCH;
END;

GO






-- stored procedure for filtration, sorting, and pagination
CREATE OR ALTER PROCEDURE usp_Member_Filter 
(
    @MemberFirstName NVARCHAR(255) = NULL,
	@MemberLastName NVARCHAR(255) = NULL,
    @ClassName NVARCHAR(255) = NULL,
	@TrainerName NVARCHAR(255) = NULL,
    @RegistrationDate DATETIME = NULL,
    @TotalCount INT OUT,
    @SortColumnName NVARCHAR(100) = 'MemberID',
    @SortDesc BIT = 0,
    @Page INT = 1, 
    @PageSize INT = 10
)
AS
BEGIN
  -- try catch
    BEGIN TRY
    -- filtering 
    DECLARE @sqlWhere NVARCHAR(1000) = ''
    DECLARE @whereParams NVARCHAR(1000) = N'@MemberFirstName NVARCHAR(255), @MemberLastName NVARCHAR(255),@ClassName NVARCHAR(255), @TrainerName NVARCHAR(255),  @RegistrationDate DATETIME, @OffsetRows INT, @PageSize INT'
    DECLARE @countParams NVARCHAR(1000) = N'@MemberFirstName NVARCHAR(255), @MemberLastName NVARCHAR(255), @ClassName NVARCHAR(255), @TrainerName NVARCHAR(255),  @RegistrationDate DATETIME, @TotalCount INT OUT'

    -- what to search for
    IF LEN(TRIM(ISNULL(@MemberFirstName, ''))) > 0
    SET @sqlWhere += ' M.FirstName LIKE ''%'' + @MemberFirstName + ''%'' AND '

    IF LEN(TRIM(ISNULL(@MemberLastName, ''))) > 0
    SET @sqlWhere += ' M.LastName LIKE ''%'' + @MemberLastName + ''%'' AND '

    IF LEN(TRIM(ISNULL(@ClassName, ''))) > 0
    SET @sqlWhere += ' C.ClassName LIKE ''%'' + @ClassName + ''%'' AND '

    IF @RegistrationDate IS NOT NULL
    SET @sqlWhere += ' M.RegistrationDate >= @RegistrationDate AND '

	IF LEN(TRIM(ISNULL(@TrainerName, ''))) > 0
	SET @sqlWhere += ' T.FirstName LIKE ''%'' + @TrainerName + ''%'' AND '

    -- cleaning
    IF LEN(@sqlWhere) > 0
    SET @sqlWhere = ' WHERE ' + LEFT(@sqlWhere, LEN(@sqlWhere) - 4)
    ELSE 
    SET @sqlWhere = ''
    -- counting total results
    DECLARE @sqlCount NVARCHAR(1000) = N'SELECT @TotalCount = COUNT(*)
                      FROM (
                      SELECT 
						DISTINCT M.MemberID
                      FROM Members M
                      JOIN MemberClasses MC ON M.MemberID = MC.MemberID
                      JOIN Classes C ON MC.ClassID = C.ClassID
					  JOIN Trainers T ON T.TrainerID = C.TrainerID'
                      + @sqlWhere + 
                      ') AS TotalRecords'
        
    
    EXEC sp_executesql @sqlCount, 
                           @countParams, 
                           @MemberFirstName = @MemberFirstName, 
                           @MemberLastName = @MemberLastName, 
						   @TrainerName = @TrainerName,
                           @ClassName = @ClassName,
                           @RegistrationDate = @RegistrationDate,
                           @TotalCount = @TotalCount OUT

-- validating column name vs the existing columns 
    IF @SortColumnName NOT IN ('MemberID', 'FirstName', 'LastName', 'ClassName', 'RegistrationDate', 'TrainerName')
      SET @SortColumnName = 'MemberID'
    -- sorting & showing people 
    DECLARE @sql NVARCHAR(2000) = N'SELECT 
										M.MemberID, 
										M.FirstName, 
										M.LastName, 
										STRING_AGG(C.ClassName, '','') AS MemberClassesList,
										STRING_AGG(T.FirstName, '','') AS TrainersList,
										M.RegistrationDate
                                    FROM Members M
                                    JOIN MemberClasses MC ON M.MemberID = MC.MemberID
                                    JOIN Classes C ON MC.ClassID = C.ClassID
									JOIN Trainers T ON T.TrainerID = C.TrainerID '
                                        + @sqlWhere
										+ ' GROUP BY M.MemberID, M.FirstName, M.LastName, M.RegistrationDate'
                                        + ' ORDER BY ' + @SortColumnName
                                        + CASE WHEN @SortDesc = 1 THEN ' DESC' ELSE ' ASC' END
                                        + ' OFFSET @OffsetRows ROWS FETCH NEXT @PageSize ROWS ONLY'
    -- make sure it's not ridiculous page number
    IF @PageSize < 1 SET @PageSize = 5
    IF @PageSize > 100 SET @PageSize = 100
    -- offset (for pagination)
        DECLARE @Offset INT = (@Page - 1) * @PageSize
        IF @Offset < 0 SET @Offset = 0
     -- final query
        EXEC sp_executesql @sql, 
                           @whereParams, 
                           @MemberFirstName = @MemberFirstName, 
                           @MemberLastName = @MemberLastName, 
                           @ClassName = @ClassName,
						   @TrainerName = @TrainerName,
                           @RegistrationDate = @RegistrationDate,
                           @OffsetRows = @Offset,
                           @PageSize = @PageSize
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
        DECLARE @ErrorState INT = ERROR_STATE()
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
    END CATCH
END

GO


-- XML export
CREATE OR ALTER PROCEDURE usp_Members_XMLExport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        M.MemberID AS '@MemberID',
        M.FirstName + ' ' + M.LastName AS '@Name',
        M.PhoneNumber AS 'PhoneNumber',
        (
            SELECT 
                T.TrainerID AS '@TrainerID',
                T.FirstName AS 'FirstName',
                T.LastName AS 'LastName'
            FROM Trainers T
            JOIN Classes C ON T.TrainerID = C.TrainerID
            JOIN MemberClasses MC ON C.ClassID = MC.ClassID
            WHERE MC.MemberID = M.MemberID
            FOR XML PATH('Trainer'), TYPE
        ) AS 'Trainers'
    FROM dbo.Members M
    FOR XML PATH('Member'), ROOT('MembersWithTrainerInfo');
END;
GO
--JSON export
CREATE OR ALTER PROCEDURE usp_Members_JSONExport
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        M.MemberID,
        M.FirstName + ' ' + M.LastName AS 'Name',
        M.PhoneNumber,
        (
            SELECT 
                T.TrainerID,
                T.FirstName,
                T.LastName
            FROM Trainers T
            JOIN Classes C ON T.TrainerID = C.TrainerID
            JOIN MemberClasses MC ON C.ClassID = MC.ClassID
            WHERE MC.MemberID = M.MemberID
            FOR JSON PATH
        ) AS Trainers
    FROM dbo.Members M
    FOR JSON PATH, ROOT('MembersWithTrainerInfo');
END;
GO

-- XML import
CREATE PROCEDURE usp_Members_XMLImport
    @Data XML
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @hDoc INT;
    EXEC sp_xml_preparedocument @hDoc OUTPUT, @Data;
    
    -- temp table for members
    DECLARE @MemberTable TABLE (
        FirstName NVARCHAR(255),
        LastName NVARCHAR(255),
        PhoneNumber NVARCHAR(20),
        DateOfBirth DATE,
        Email NVARCHAR(255),
        ProfilePicture VARBINARY(MAX),
        IsMale BIT
    );
    
    INSERT INTO @MemberTable (FirstName, LastName, PhoneNumber, DateOfBirth, Email, ProfilePicture, IsMale)
    SELECT 
        FirstName,
        LastName,
        PhoneNumber,
        DateOfBirth,
        Email,
        ProfilePicture,
        IsMale
    FROM OPENXML(@hDoc, '/Data/Members/Member', 2)
    WITH (
        FirstName NVARCHAR(255) '@FirstName',
        LastName NVARCHAR(255) '@LastName',
        PhoneNumber NVARCHAR(20) '@PhoneNumber',
        DateOfBirth DATE '@DateOfBirth',
        Email NVARCHAR(255) '@Email',
        ProfilePicture VARBINARY(MAX) '@ProfilePicture',
        IsMale BIT '@IsMale'
    );
    
    INSERT INTO Members (FirstName, LastName, PhoneNumber, DateOfBirth, Email, ProfilePicture, IsMale)
    SELECT FirstName, LastName, PhoneNumber, DateOfBirth, Email, ProfilePicture, IsMale FROM @MemberTable;
    
    -- temp table for trainers
    DECLARE @TrainerTable TABLE (
        FirstName NVARCHAR(255),
        LastName NVARCHAR(255),
        PhoneNumber NVARCHAR(20),
        Email NVARCHAR(255),
        ExperienceYears INT,
        IsMale BIT,
        Salary DECIMAL(10,2)
    );
    
    INSERT INTO @TrainerTable (FirstName, LastName, PhoneNumber, Email, ExperienceYears, IsMale, Salary)
    SELECT 
        FirstName,
        LastName,
        PhoneNumber,
        Email,
        ExperienceYears,
        IsMale,
        Salary
    FROM OPENXML(@hDoc, '/Data/Trainers/Trainer', 2)
    WITH (
        FirstName NVARCHAR(255) '@FirstName',
        LastName NVARCHAR(255) '@LastName',
        PhoneNumber NVARCHAR(20) '@PhoneNumber',
        Email NVARCHAR(255) '@Email',
        ExperienceYears INT '@ExperienceYears',
        IsMale BIT '@IsMale',
        Salary DECIMAL(10,2) '@Salary'
    );
    
    INSERT INTO Trainers (FirstName, LastName, PhoneNumber, Email, ExperienceYears, IsMale, Salary)
    SELECT FirstName, LastName, PhoneNumber, Email, ExperienceYears, IsMale, Salary FROM @TrainerTable;
    
    EXEC sp_xml_removedocument @hDoc;
    PRINT 'Import Successful';
END;
GO
--JSON import
CREATE PROCEDURE usp_Members_JSONImport
    @JsonData NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    
    -- temp table for members
    DECLARE @MemberTable TABLE (
        FirstName NVARCHAR(255),
        LastName NVARCHAR(255),
        PhoneNumber NVARCHAR(20),
        DateOfBirth DATE,
        Email NVARCHAR(255),
        ProfilePicture VARBINARY(MAX),
        IsMale BIT
    );
    
    INSERT INTO @MemberTable (FirstName, LastName, PhoneNumber, DateOfBirth, Email, ProfilePicture, IsMale)
    SELECT 
        FirstName,
        LastName,
        PhoneNumber,
        DateOfBirth,
        Email,
        ProfilePicture,
        IsMale
    FROM OPENJSON(@JsonData, '$.Members')
    WITH (
        FirstName NVARCHAR(255) '$.FirstName',
        LastName NVARCHAR(255) '$.LastName',
        PhoneNumber NVARCHAR(20) '$.PhoneNumber',
        DateOfBirth DATE '$.DateOfBirth',
        Email NVARCHAR(255) '$.Email',
        ProfilePicture VARBINARY(MAX) '$.ProfilePicture',
        IsMale BIT '$.IsMale'
    );
    
    INSERT INTO Members (FirstName, LastName, PhoneNumber, DateOfBirth, Email, ProfilePicture, IsMale)
    SELECT FirstName, LastName, PhoneNumber, DateOfBirth, Email, ProfilePicture, IsMale FROM @MemberTable;
    
    -- temp table for trainers
    DECLARE @TrainerTable TABLE (
        FirstName NVARCHAR(255),
        LastName NVARCHAR(255),
        PhoneNumber NVARCHAR(20),
        Email NVARCHAR(255),
        ExperienceYears INT,
        IsMale BIT,
        Salary DECIMAL(10,2)
    );
    
    INSERT INTO @TrainerTable (FirstName, LastName, PhoneNumber, Email, ExperienceYears, IsMale, Salary)
    SELECT 
        FirstName,
        LastName,
        PhoneNumber,
        Email,
        ExperienceYears,
        IsMale,
        Salary
    FROM OPENJSON(@JsonData, '$.Trainers')
    WITH (
        FirstName NVARCHAR(255) '$.FirstName',
        LastName NVARCHAR(255) '$.LastName',
        PhoneNumber NVARCHAR(20) '$.PhoneNumber',
        Email NVARCHAR(255) '$.Email',
        ExperienceYears INT '$.ExperienceYears',
        IsMale BIT '$.IsMale',
        Salary DECIMAL(10,2) '$.Salary'
    );
    
    INSERT INTO Trainers (FirstName, LastName, PhoneNumber, Email, ExperienceYears, IsMale, Salary)
    SELECT FirstName, LastName, PhoneNumber, Email, ExperienceYears, IsMale, Salary FROM @TrainerTable;
    
    PRINT 'Import Successful';
END;
GO

