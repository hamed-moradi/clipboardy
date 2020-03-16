CREATE TABLE [dbo].[ContentType] (
    [Id]        INT           NOT NULL,
    [Name]      VARCHAR (128) NOT NULL,
    [Extension] VARCHAR (16)  NOT NULL,
    [MIMEType]  VARCHAR (256) NOT NULL,
    CONSTRAINT [PK_ContentType] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [IX_ContentType] UNIQUE NONCLUSTERED ([Extension] ASC)
);

