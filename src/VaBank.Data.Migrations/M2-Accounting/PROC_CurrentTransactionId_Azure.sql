SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[CurrentTransactionId]
@transaction_id BIGINT OUTPUT
AS
BEGIN TRANSACTION Dummy
SELECT @transaction_id = [transaction_id] FROM [sys].[dm_tran_session_transactions]
WHERE [session_id] = @@spid
COMMIT TRANSACTION Dummy