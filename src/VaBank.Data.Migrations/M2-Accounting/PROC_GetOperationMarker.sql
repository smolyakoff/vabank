SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [App].[GetOperationMarker]()
RETURNS @OperationMarker TABLE
(
	Id uniqueidentifier,
	TransactionI bigint,
	TimestampUtc datetime,
	Name nvarchar(100),
	Finished bit,
	DbUser nvarchar(256),
	DbApplication varchar(256),
	AppUserId uniqueidentifier,
	AppClientId nvarchar(100)
)
AS
BEGIN
	DECLARE @currentTransactionId BIGINT =
	 (SELECT [transaction_id] FROM [sys].[dm_tran_current_transaction])
	INSERT @OperationMarker
	SELECT TOP 1 [Id]
      ,[TransactionId]
      ,[TimestampUtc]
      ,[Name]
      ,[Finished]
      ,[DbUser]
      ,[DbApplication]
      ,[AppUserId]
      ,[AppClientId]
  FROM [App].[OperationMarker] WITH (NOLOCK)
  WHERE [TransactionId] = @currentTransactionId AND [Finished] = 0
  ORDER BY [TimestampUtc] DESC
  RETURN
END
GO