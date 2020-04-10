CREATE TABLE [dbo].[Clipboard] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [AccountId] INT           NOT NULL,
    [DeviceId]  INT           NOT NULL,
    [TypeId]    INT           NOT NULL,
    [Content]   VARCHAR (MAX) NOT NULL,
    [Priority]  DATETIME      CONSTRAINT [DF_Clipboard_Priority] DEFAULT (getdate()) NOT NULL,
    [CreatedAt] DATETIME      CONSTRAINT [DF_Clipboard_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [StatusId]  INT           CONSTRAINT [DF_Clipboard_StatusId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_Clipboard] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Clipboard_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_Clipboard_AccountDevice] FOREIGN KEY ([DeviceId]) REFERENCES [dbo].[AccountDevice] ([Id]),
    CONSTRAINT [FK_Clipboard_ContentType] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[ContentType] ([Id]),
    CONSTRAINT [FK_Clipboard_GeneralStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[GeneralStatus] ([Id])
);



