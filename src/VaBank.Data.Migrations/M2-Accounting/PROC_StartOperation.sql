SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[StartOperation]
	@Id UNIQUEIDENTIFIER = NULL OUTPUT,
	@StartedUtc DATETIME = NULL,
	@Name NVARCHAR(50) = NULL,
	@AppUserId UNIQUEIDENTIFIER = NULL,
	@AppClientId NVARCHAR(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	INSERT INTO [App].[Operation] 
		   ([StartedUtc], [Name], [AppUserId], [AppClientId])
	VALUES (ISNULL(@StartedUtc, GETUTCDATE()), ISNULL(@Name, N'DB-CHANGE'), @AppUserId, @AppClientId)
	EXEC [App].[CurrentOperationId] @Id = @Id OUTPUT
END
GO