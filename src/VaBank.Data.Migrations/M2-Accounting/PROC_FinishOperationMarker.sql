SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[FinishOperationMarker]
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @currentTransactionId BIGINT =
	 (SELECT [transaction_id] FROM [sys].[dm_tran_current_transaction])
	DECLARE @id UNIQUEIDENTIFIER = (
		SELECT TOP 1 [Id]
		FROM [App].[OperationMarker]
		WHERE [TransactionId] = @currentTransactionId AND [Finished] = 0
		ORDER BY [TimestampUtc] DESC)
	UPDATE [App].[OperationMarker] 
	SET [Finished] = 1
	WHERE [Id] = @id
END
GO