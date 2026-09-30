CREATE TABLE [dbo].[Users]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserName] NVARCHAR(16) NOT NULL,
    [FirstName] NVARCHAR(50) NOT NULL,
    [LastName] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(16) NOT NULL,

    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
