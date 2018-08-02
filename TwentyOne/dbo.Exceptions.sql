CREATE TABLE [dbo].[Exceptions] (
    [Id]               INT NOT NULL IDENTITY,
    [ExceptionType]    VARCHAR (50)  NULL,
    [ExceptionMessage] VARCHAR (200) NULL,
    [TimeStamp]        DATETIME      NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

