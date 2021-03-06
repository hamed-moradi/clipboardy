﻿CREATE TABLE [dbo].[AccountProfile] (
    [Id]                  INT           IDENTITY (1, 1) NOT NULL,
    [AccountId]           INT           NOT NULL,
    [TypeId]              INT           NOT NULL,
    [LinkedId]            VARCHAR (128) NULL,
    [CreatedAt]           DATETIME      CONSTRAINT [DF_AccountProfile_CreatedAt] DEFAULT (getdate()) NOT NULL,
    [StatusId]            INT           CONSTRAINT [DF_AccountProfile_StatusId] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_AccountProfile] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountProfile_Account] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
    CONSTRAINT [FK_AccountProfile_GeneralStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[GeneralStatus] ([Id]),
    CONSTRAINT [FK_AccountProfile_TypeId] FOREIGN KEY ([TypeId]) REFERENCES [dbo].[AccountProfileType] ([Id])
);





