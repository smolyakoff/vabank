SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[StartOperation]
	@Id UNIQUEIDENTIFIER = NULL OUTPUT,
	@TimestampUtc DATETIME = NULL,
	@Name NVARCHAR(50) = NULL,
	@AppUserId UNIQUEIDENTIFIER = NULL,
	@AppClientId NVARCHAR(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [App].[Operation] 
		   ([TimestampUtc], [Name], [AppUserId], [AppClientId])
	VALUES (ISNULL(@TimestampUtc, GETUTCDATE()), ISNULL(@Name, N'DB-CHANGE'), @AppUserId, @AppClientId)
	SET @Id = (SELECT [Id] FROM [App].[CurrentOperation])
END
GO