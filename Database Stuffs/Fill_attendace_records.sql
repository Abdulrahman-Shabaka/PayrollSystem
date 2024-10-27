-- Cursor to go through each employee in the Employees table
DECLARE EmployeeCursor CURSOR FOR 
SELECT id, HireDate 
FROM [payroll_db].[dbo].[Employees];  -- Use your actual Employees table name here

OPEN EmployeeCursor;

DECLARE @EmployeeId INT;
DECLARE @HireDate DATE;

-- Loop through each employee
FETCH NEXT FROM EmployeeCursor INTO @EmployeeId, @HireDate;
WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @CurrentDate DATE = @HireDate;

    -- Loop through each day from the hire date to today
    WHILE @CurrentDate <= GETDATE()
    BEGIN
        -- Check if a record already exists for this employee on the current date
        IF NOT EXISTS (
            SELECT 1 
            FROM [payroll_db].[dbo].[Attendances] 
            WHERE [EmployeeId] = @EmployeeId AND [Date] = @CurrentDate
        )
        BEGIN
            DECLARE @Status INT = CASE WHEN RAND() < 0.8 THEN 1 ELSE 2 END; -- 80% chance of 'Present'
            DECLARE @LoginTime TIME = CASE WHEN @Status = 1 THEN CAST(CONCAT(8 + FLOOR(RAND() * 2), ':', FLOOR(RAND() * 60)) AS TIME) ELSE NULL END;
            DECLARE @LogoutTime TIME = CASE WHEN @Status = 1 THEN CAST(CONCAT(16 + FLOOR(RAND() * 2), ':', FLOOR(RAND() * 60)) AS TIME) ELSE NULL END;

            -- Insert attendance record for the employee
            INSERT INTO [payroll_db].[dbo].[Attendances] ([EmployeeId], [Date], [Status], [Reason], [LoginTime], [LogoutTime])
            VALUES (@EmployeeId, @CurrentDate, @Status, 
                    CASE @Status WHEN 1 THEN NULL ELSE 'Sick leave' END, 
                    @LoginTime, @LogoutTime);
        END;

        -- Move to the next date
        SET @CurrentDate = DATEADD(DAY, 1, @CurrentDate);
    END;

    -- Move to the next employee
    FETCH NEXT FROM EmployeeCursor INTO @EmployeeId, @HireDate;
END;

CLOSE EmployeeCursor;
DEALLOCATE EmployeeCursor;
