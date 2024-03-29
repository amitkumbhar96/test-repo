USE [EmployeeDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_InsUpdDelEmployees]    Script Date: 10/22/2019 11:02:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[sp_InsUpdDelEmployees]  
    @ID INT ,  
    @FirstName NVARCHAR(50) ,  
    @LastName NVARCHAR(50) ,  
    @Gender NVARCHAR(50) ,  
    @Salary NVARCHAR(50),
	@type VARCHAR(10)
AS   
    BEGIN  
        IF ( @type = 'Ins' )   
            BEGIN  
                INSERT  INTO Employees  
                VALUES  ( @FirstName, @LastName, @Gender, @Salary)  
            END  
        IF ( @type = 'Upd' )   
            BEGIN  
                UPDATE  Employees  
                SET     FirstName = @FirstName ,  
                        LastName= @LastName ,  
                        Gender = @Gender ,  
                        Salary = @Salary 
                WHERE   Id = @ID  
            END  
        IF ( @type = 'Del' )   
            BEGIN  
                DELETE  FROM Employees  
                WHERE   Id = @ID 
            END   
        IF ( @type = 'GetById' )   
            BEGIN  
                SELECT  *  
                FROM    Employees  
                WHERE   Id = @ID 
            END
		IF(@type='All')
		BEGIN
		SELECT * FROM Employees
		END
        SELECT  *  
        FROM    Employees  
    END  