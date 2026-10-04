CREATE TABLE [dbo].[Posts]
(
    [Id] INT IDENTITY(1,1) NOT NULL,
    [UserId] INT NOT NULL,
    [Title] NVARCHAR(150) NOT NULL,
    [Body] TEXT NOT NULL,
    [DateCreated] DATETIME2 NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Posts_Users]
        FOREIGN KEY ([UserId])
        REFERENCES [dbo].[Users] ([Id])
);