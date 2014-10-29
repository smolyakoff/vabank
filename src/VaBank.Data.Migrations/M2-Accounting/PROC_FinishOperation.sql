SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[FinishOperation]
	@Id uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF @Id IS NULL
	BEGIN
		DECLARE @currentTransactionId BIGINT =
			(SELECT [transaction_id] FROM [sys].[dm_tran_current_transaction])
		SET @Id = (
			SELECT TOP 1 [Id]
			FROM [App].[Operation]
			WHERE [TransactionId] = @currentTransactionId AND [Finished] = 0
			ORDER BY [StartedUtc] DESC)
	END;
	UPDATE [App].[Operation] 
	SET [Finished] = 1, [FinishedUtc] = GETUTCDATE()
	WHERE [Id] = @Id
END
GO