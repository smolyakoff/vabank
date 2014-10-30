SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [App].[CurrentTransactionId]
@transaction_id BIGINT OUTPUT
AS
SELECT @transaction_id = [transaction_id] FROM [sys].[dm_tran_current_transaction]