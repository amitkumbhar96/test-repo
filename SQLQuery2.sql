Create Table UserLogin(
userId INT PRIMARY KEY IDENTITY(10,1),
Email VARCHAR(254) NOT NULL,
Password NVARCHAR(MAX),
RememberMe BIT
);