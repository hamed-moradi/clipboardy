CREATE TABLE [dbo].[AccountProfileType] (
    [Id]          INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (32)  NOT NULL,
    [Description] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_AccountProfileType_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);

