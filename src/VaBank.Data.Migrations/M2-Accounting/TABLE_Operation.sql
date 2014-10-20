/****** Object:  Table [App].[Operation]    Script Date: 13.10.2014 23:46:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [App].[Operation](
	[Id] [uniqueidentifier] NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[StartedUtc] [datetime] NOT NULL,
	[Name] [nvarchar] (100) NOT NULL,
	[Finished] [bit] NOT NULL,
	[FinishedUtc] [datetime] NULL,
	[DbUser] [nvarchar](256) NOT NULL,
	[DbApplication] [varchar](256) NOT NULL,
	[AppUserId] [uniqueidentifier] NULL,
	[AppClientId] [nvarchar](100) NULL,
 CONSTRAINT [PK_Operation] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [App].[Operation] ADD  CONSTRAINT [DF_Operation_Id]  DEFAULT (newsequentialid()) FOR [Id]
GO

ALTER TABLE [App].[Operation] ADD  CONSTRAINT [DF_Operation_StartedUtc]  DEFAULT (getutcdate()) FOR [StartedUtc]
GO

ALTER TABLE [App].[Operation] ADD  CONSTRAINT [DF_Operation_Name]  DEFAULT (N'DB-CHANGE') FOR [Name]
GO

ALTER TABLE [App].[Operation] ADD  CONSTRAINT [DF_Operation_Finished]  DEFAULT (0) FOR [Finished]
GO

ALTER TABLE [App].[Operation] ADD  CONSTRAINT [DF_Operation_SqlUser]  DEFAULT (suser_sname()) FOR [DbUser]
GO

ALTER TABLE [App].[Operation] ADD  CONSTRAINT [DF_Operation_DbApplication]  DEFAULT (app_name()) FOR [DbApplication]
GO