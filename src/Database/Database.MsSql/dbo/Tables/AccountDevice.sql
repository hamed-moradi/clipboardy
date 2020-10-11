CREATE TABLE [dbo].[AccountDevice] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [AccountId]  INT           NOT NULL,
    [DeviceId]   VARCHAR (128) NOT NULL,
    [DeviceName] VARCHAR (128) NOT NULL,
    [DeviceType] VARCHAR (128) NOT NULL,
    [CreatedAt]  DATETIME      CONSTRAINT [DF_AccountDevice_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [StatusId]   INT           CONSTRAINT [DF_AccountDevice_StatusId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AccountDevice] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountDevice_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_AccountDevice_GeneralStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[GeneralStatus] ([Id]),
    CONSTRAINT [IX_AccountDevice_DeviceId] UNIQUE NONCLUSTERED ([DeviceId] ASC)
);



