SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[GenerateHistoryTable]
	@TableName varchar(128),
	@Owner varchar(128) = 'dbo',
	@HistoryNameExtension varchar(128) = '_History',
	@DropHistoryTable bit = 0
AS
BEGIN

	-- Check if table exists
	IF not exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		PRINT 'ERROR: Table does not exist'
		RETURN
	END

	-- Check @HistoryNameExtension
	IF @HistoryNameExtension is null
	BEGIN
		PRINT 'ERROR: @HistoryNameExtension cannot be null'
		RETURN
	END

	-- Drop audit table if it exists and drop should be forced
	IF (exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1) and @DropHistoryTable = 1)
	BEGIN
		PRINT 'Dropping audit table [' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']'
		EXEC ('drop table ' + @TableName + @HistoryNameExtension)
	END

	-- Declare cursor to loop over columns
	DECLARE TableColumns CURSOR Read_Only
	FOR SELECT b.name, c.name as TypeName, b.length, b.isnullable, b.collation, b.xprec, b.xscale
		FROM sysobjects a 
		inner join syscolumns b on a.id = b.id 
		inner join systypes c on b.xtype = c.xtype and c.name <> 'sysname' 
		WHERE a.id = object_id(N'[' + @Owner + '].[' + @TableName + ']') 
		and OBJECTPROPERTY(a.id, N'IsUserTable') = 1 
		ORDER BY b.colId

	OPEN TableColumns

	-- Declare temp variable to fetch records into
	DECLARE @ColumnName varchar(128)
	DECLARE @ColumnType varchar(128)
	DECLARE @ColumnLength smallint
	DECLARE @ColumnNullable int
	DECLARE @ColumnCollation sysname
	DECLARE @ColumnPrecision tinyint
	DECLARE @ColumnScale tinyint

	-- Declare variable to build statements
	DECLARE @CreateStatement varchar(8000)
	DECLARE @ListOfFields varchar(2000)
	SET @ListOfFields = ''


	-- Check if audit table exists
	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	BEGIN
		-- AuditTable exists, update needed
		PRINT 'Table already exists. Only triggers will be updated.'

		FETCH Next FROM TableColumns
		INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@ColumnType <> 'text' and @ColumnType <> 'ntext' and @ColumnType <> 'image' and @ColumnType <> 'timestamp')
			BEGIN
				SET @ListOfFields = @ListOfFields + @ColumnName + ','
			END

			FETCH Next FROM TableColumns
			INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale

		END
	END
	ELSE
	BEGIN
		-- AuditTable does not exist, create new

		-- Start of create table
		SET @CreateStatement = 'CREATE TABLE [' + @Owner + '].[' + @TableName + @HistoryNameExtension + '] ('
		SET @CreateStatement = @CreateStatement + '[HistoryId] [bigint] IDENTITY (1, 1) NOT NULL,'

		FETCH Next FROM TableColumns
		INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		
		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF (@ColumnType <> 'text' and @ColumnType <> 'ntext' and @ColumnType <> 'image' and @ColumnType <> 'timestamp')
			BEGIN
				SET @ListOfFields = @ListOfFields + @ColumnName + ','
		
				SET @CreateStatement = @CreateStatement + '[' + @ColumnName + '] [' + @ColumnType + '] '
				
				IF @ColumnType in ('binary', 'char', 'nchar', 'nvarchar', 'varbinary', 'varchar')
				BEGIN
					IF (@ColumnLength = -1)
						Set @CreateStatement = @CreateStatement + '(max) '	 	
					ELSE
						SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnLength as varchar(10)) + ') '	 	
				END
		
				IF @ColumnType in ('decimal', 'numeric')
					SET @CreateStatement = @CreateStatement + '(' + cast(@ColumnPrecision as varchar(10)) + ',' + cast(@ColumnScale as varchar(10)) + ') '	 	
		
				IF @ColumnType in ('char', 'nchar', 'nvarchar', 'varchar', 'text', 'ntext')
					SET @CreateStatement = @CreateStatement + 'COLLATE ' + @ColumnCollation + ' '
		
				IF @ColumnNullable = 0
					SET @CreateStatement = @CreateStatement + 'NOT '	 	
		
				SET @CreateStatement = @CreateStatement + 'NULL, '	 	
			END

			FETCH Next FROM TableColumns
			INTO @ColumnName, @ColumnType, @ColumnLength, @ColumnNullable, @ColumnCollation, @ColumnPrecision, @ColumnScale
		END
		
		-- Add audit trail columns
		SET @CreateStatement = @CreateStatement + '[HistoryAction] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,'
		SET @CreateStatement = @CreateStatement + '[HistoryTimestampUtc] [datetime] NOT NULL,'
		SET @CreateStatement = @CreateStatement + '[HistoryOperationId] [uniqueidentifier] NOT NULL)'

		-- Create audit table
		PRINT 'Creating history table [' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']'
		EXEC (@CreateStatement)

		-- Set primary key and default values
		SET @CreateStatement = 'ALTER TABLE [' + @Owner + '].[' + @TableName + @HistoryNameExtension + '] ADD '
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [DF_' + @TableName + @HistoryNameExtension + '_HistoryTimestampUtc] DEFAULT (GETUTCDATE()) FOR [HistoryTimestampUtc],'
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [PK_' + @TableName + @HistoryNameExtension + '] PRIMARY KEY CLUSTERED ([HistoryId]) ON [PRIMARY]'

		EXEC (@CreateStatement)

		-- Create foreign key to operation marker
		SET @CreateStatement = 'ALTER TABLE [' + @Owner + '].[' + @TableName + @HistoryNameExtension + '] ADD '
		SET @CreateStatement = @CreateStatement + 'CONSTRAINT [FK_' + @TableName + @HistoryNameExtension + '_To_Operation] FOREIGN KEY ([HistoryOperationId]) REFERENCES [App].[Operation]([Id])' 
		EXEC (@CreateStatement)

		-- Create indexes
		SET @CreateStatement = 'CREATE INDEX [IX_' + @TableName + @HistoryNameExtension + '_HistoryTimestampUtc] ON [' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']([HistoryTimestampUtc] DESC)' 
		EXEC (@CreateStatement)
		SET @CreateStatement = 'CREATE INDEX [IX_' + @TableName + @HistoryNameExtension + '_HistoryOperationId] ON [' + @Owner + '].[' + @TableName + @HistoryNameExtension + ']([HistoryOperationId])' 
		EXEC (@CreateStatement)
	END

	CLOSE TableColumns
	DEALLOCATE TableColumns

	/* Drop Triggers, if they exist */
	PRINT 'Dropping triggers'
	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TRG_' + @TableName + '_Insert_History]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TRG_' + @TableName + '_Insert_History]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TRG_' + @TableName + '_Update_History]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TRG_' + @TableName + '_Update_History]')

	IF exists (SELECT * FROM dbo.sysobjects WHERE id = object_id(N'[' + @Owner + '].[TRG_' + @TableName + '_Delete_History]') and OBJECTPROPERTY(id, N'IsTrigger') = 1) 
		EXEC ('drop trigger [' + @Owner + '].[TRG_' + @TableName + '_Delete_History]')

	/* Create triggers */
	PRINT 'Creating triggers' 
	DECLARE @PreHistoryStatement VARCHAR(500) = 
		'BEGIN TRANSACTION HistoryTransaction ' + 
	    'DECLARE @operationId uniqueidentifier = (SELECT Id FROM [App].[CurrentOperation]) ' +
		'DECLARE @isNewOperation bit = 0 ' +
		'IF @operationId IS NULL SET @isNewOperation = 1 ' +
		'IF @isNewOperation = 1 EXEC [App].[StartOperation] @Id = @operationId OUTPUT '
	DECLARE @PostHistoryStatement VARCHAR(500) =
		'IF @isNewOperation = 1 UPDATE [App].[Operation] SET [Finished] = 1 WHERE [Id] = @operationId ' + 
		'COMMIT TRANSACTION HistoryTransaction'

	SET @CreateStatement = 
		'CREATE TRIGGER [TRG_' + @TableName + '_Insert_History] ON [' + @Owner + '].[' + @TableName + '] FOR INSERT AS ' + 
		@PreHistoryStatement + 
		'INSERT INTO ' + @TableName + @HistoryNameExtension + 
	    '(' +  @ListOfFields + 'HistoryAction, HistoryOperationId) ' + 
		'SELECT ' + @ListOfFields + '''I'', @operationId FROM [Inserted] ' +
		@PostHistoryStatement 
	EXEC (@CreateStatement)

	SET @CreateStatement = 
		'CREATE TRIGGER [TRG_' + @TableName + '_Update_History] ON [' + @Owner + '].[' + @TableName + '] FOR UPDATE AS ' + 
		@PreHistoryStatement + 
		'INSERT INTO ' + @TableName + @HistoryNameExtension + 
	    '(' +  @ListOfFields + 'HistoryAction, HistoryOperationId) ' + 
		'SELECT ' + @ListOfFields + '''U'', @operationId FROM [Inserted] ' +
		@PostHistoryStatement 
	EXEC (@CreateStatement)

	SET @CreateStatement = 
		'CREATE TRIGGER [TRG_' + @TableName + '_Delete_History] ON [' + @Owner + '].[' + @TableName + '] FOR DELETE AS ' + 
		@PreHistoryStatement + 
		'INSERT INTO ' + @TableName + @HistoryNameExtension + 
	    '(' +  @ListOfFields + 'HistoryAction, HistoryOperationId) ' + 
		'SELECT ' + @ListOfFields + '''D'', @operationId FROM [Deleted] ' +
		@PostHistoryStatement 
	EXEC (@CreateStatement)
END