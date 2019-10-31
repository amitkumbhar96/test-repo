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
