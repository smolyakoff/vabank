/****** Object:  View [App].[CurrentOperation]    Script Date: 10/15/2014 12:43:15 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [App].[CurrentOperation]
AS
SELECT TOP (1) ID, TransactionID, StartedUtc, Name, Finished, DbUser, DbApplication, AppUserID, AppClientID
FROM App.Operation WITH (NOLOCK)
WHERE Finished = 0 AND 
	 (TransactionID = (SELECT transaction_id FROM sys.dm_tran_current_transaction))
ORDER BY StartedUtc DESC