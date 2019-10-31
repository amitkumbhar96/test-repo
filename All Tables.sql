CREATE TABLE [dbo].[Registration]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	[FirstName] VARCHAR(50) NOT NULL,
	[LastName] VARCHAR(50) NOT NULL,
	[Email] VARCHAR(100) NOT NULL,
	[Password] NVARCHAR(MAX) NOT NULL,
	IsEmailVerified BIT,
	ActivationCode uniqueidentifier
)
Create Table UserLogin(
userId INT PRIMARY KEY IDENTITY(10,1),
Email VARCHAR(254) NOT NULL,
Password NVARCHAR(MAX),
RememberMe BIT
);

Create Table Employee(
Id INT PRIMARY KEY IDENTITY(10,1),
FirstName VARCHAR(254) NOT NULL,
LastName VARCHAR(254) NOT NULL,
Gender VARCHAR(20) NOT NULL,
Salary INT NOT NULL

);