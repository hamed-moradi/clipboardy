CREATE TABLE [dbo].[AccountProfile] (
    [Id]             INT           IDENTITY (1, 1) NOT NULL,
    [AccountId]      INT           NOT NULL,
    [Email]          VARCHAR (128) NULL,
    [ConfirmedEmail] BIT           CONSTRAINT [DF_AccountProfile_ConfirmedEmail] DEFAULT ((0)) NOT NULL,
    [Phone]          VARCHAR (16)  NULL,
    [ConfirmedPhone] BIT           CONSTRAINT [DF_AccountProfile_ConfirmedPhone] DEFAULT ((0)) NOT NULL,
    [CreatedAt]      DATETIME      CONSTRAINT [DF_AccountProfile_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [StatusId]       INT           CONSTRAINT [DF_AccountProfile_StatusId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AccountProfile] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountProfile_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_AccountProfile_GeneralStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[GeneralStatus] ([Id]),
    CONSTRAINT [IX_AccountProfile_Email] UNIQUE NONCLUSTERED ([Email] ASC),
    CONSTRAINT [IX_AccountProfile_Phone] UNIQUE NONCLUSTERED ([Phone] ASC)
);

