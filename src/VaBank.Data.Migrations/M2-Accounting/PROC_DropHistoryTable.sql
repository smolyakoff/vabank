SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[DropHistoryTable]
	@TableName varchar(128),
	@Owner varchar(128) = 'dbo',
	@HistoryNameExtension varchar(128) = '_History'
AS
BEGIN
	IF @TableName is null
	BEGIN
		PRINT 'ERROR: @TableName cannot be null'
		RETURN
	END


	IF @HistoryNameExtension is null
	BEGIN
		PRINT 'ERROR: @HistoryNameExtension cannot be null'
		RETURN
	END

	-- Drop audit table if it exists and drop should be forced
	IF (exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1))
	BEGIN
		PRINT 'Dropping audit table [' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']'
		EXEC ('drop table [' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']')
	END

	/* Drop Triggers, if they exist */
	PRINT 'Dropping triggers'
	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TRG_' + @TableName + '_Insert_History]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TRG_' + @TableName + '_Insert_History]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TRG_' + @TableName + '_Update_History]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TRG_' + @TableName + '_Update_History]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TRG_' + @TableName + '_Delete_History]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TRG_' + @TableName + '_Delete_History]')


	
END