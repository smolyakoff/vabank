SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROC [App].[CurrentOperationId]
@ID UNIQUEIDENTIFIER OUTPUT
AS
BEGIN
DECLARE @currentTransactionId BIGINT
EXEC [App].[CurrentTransactionId] @transaction_id = @currentTransactionId OUTPUT
SELECT TOP (1) @Id = ID
FROM [App].[Operation] WITH (NOLOCK)
WHERE Finished = 0 AND (TransactionID = @currentTransactionId)
ORDER BY StartedUtc DESC
RETURN
END