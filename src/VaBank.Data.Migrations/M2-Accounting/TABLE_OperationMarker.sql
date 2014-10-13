/****** Object:  Table [App].[OperationMarker]    Script Date: 13.10.2014 23:46:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [App].[OperationMarker](
	[Id] [uniqueidentifier] NOT NULL,
	[TransactionId] [bigint] NOT NULL,
	[TimestampUtc] [datetime] NOT NULL,
	[DbUser] [nvarchar](256) NOT NULL,
	[DbApplication] [varchar](256) NOT NULL,
	[AppUserId] [uniqueidentifier] NULL,
	[AppClientId] [nvarchar](100) NULL,
 CONSTRAINT [PK_OperationMarker] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [App].[OperationMarker] ADD  CONSTRAINT [DF_OperationMarker_Id]  DEFAULT (newid()) FOR [Id]
GO

ALTER TABLE [App].[OperationMarker] ADD  CONSTRAINT [DF_OperationMarker_TimestampUtc]  DEFAULT (getutcdate()) FOR [TimestampUtc]
GO

ALTER TABLE [App].[OperationMarker] ADD  CONSTRAINT [DF_OperationMarker_SqlUser]  DEFAULT (suser_sname()) FOR [DbUser]
GO

ALTER TABLE [App].[OperationMarker] ADD  CONSTRAINT [DF_OperationMarker_DbApplication]  DEFAULT (app_name()) FOR [DbApplication]
GO