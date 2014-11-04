ALTER TABLE [Accounting].[ExchangeRate]
	ADD CONSTRAINT UQ_FromToCurrencyTimeStamp 
		UNIQUE(FromCurrencyISOName, ToCurrencyISOName, TimeStampUtc);