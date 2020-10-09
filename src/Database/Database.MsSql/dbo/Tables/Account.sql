CREATE TABLE [dbo].[Account] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [Username]       VARCHAR (32)  NOT NULL,
    [Password]       VARCHAR (512) NULL,
    [ProviderId]     INT           NOT NULL,
    [LastSignedinAt] DATETIME      NULL,
    [CreatedAt]      DATETIME      CONSTRAINT [DF_Account_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [StatusId]       INT           CONSTRAINT [DF_Account_StatusId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Account] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Account_AccountProvider] FOREIGN KEY ([ProviderId]) REFERENCES [dbo].[AccountProvider] ([Id]),
    CONSTRAINT [FK_Account_GeneralStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[GeneralStatus] ([Id]),
    CONSTRAINT [IX_Account] UNIQUE NONCLUSTERED ([Username] ASC)
);



