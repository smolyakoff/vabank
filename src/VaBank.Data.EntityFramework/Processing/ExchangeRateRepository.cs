using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using MoreLinq;
using VaBank.Common.Data.Database;
using VaBank.Common.Validation;
using VaBank.Core.Processing.Entities;
using VaBank.Core.Processing.Repositories;
using VaBank.Data.EntityFramework.Common;

namespace VaBank.Data.EntityFramework.Processing
{
    public class ExchangeRateRepository : Repository<ExchangeRate>, IExchangeRateRepository
    {
        private readonly IDatabaseProvider _databaseProvider;

        public ExchangeRateRepository(DbContext context, IDatabaseProvider databaseProvider) : base(context)
        {
            Argument.NotNull(databaseProvider, "databaseProvider");
            _databaseProvider = databaseProvider;
        }

        public void Save(ExchangeRate exchangeRate)
        {
            Argument.NotNull(exchangeRate, "");
            EnsureRepositoryException(() =>
            {
                var baseCurrency = _databaseProvider.CreateParameter();
                baseCurrency.ParameterName = "@base";
                baseCurrency.DbType = DbType.String;
                baseCurrency.Value = exchangeRate.Base.ISOName;
                var foreignCurrency = _databaseProvider.CreateParameter();
                foreignCurrency.ParameterName = "@foreign";
                foreignCurrency.DbType = DbType.String;
                foreignCurrency.Value = exchangeRate.Foreign.ISOName;
                var timestamp = _databaseProvider.CreateParameter();
                timestamp.ParameterName = "@timestamp";
                timestamp.DbType = DbType.DateTime;
                timestamp.Value = exchangeRate.TimestampUtc;
                const string sql =
                    "UPDATE [Processing].[ExchangeRate] SET [IsActual] = 0 " +
                    "WHERE [BaseCurrencyISOName] = @base AND [ForeignCurrencyISOName] = @foreign AND [TimestampUtc] < @timestamp";
                Context.Database.ExecuteSqlCommand(TransactionalBehavior.EnsureTransaction, sql, baseCurrency,
                    foreignCurrency, timestamp);
                Context.Set<ExchangeRate>().Add(exchangeRate);
                return exchangeRate;
            });
        }

        public IList<ExchangeRate> GetActualForExchange(ExchangeRateKey key)
        {
            return EnsureRepositoryException(() =>
            {
                var rate1 = Context.Set<ExchangeRate>()
                    .OrderByDescending(x => x.TimestampUtc)
                    .FirstOrDefault(x => x.Base.ISOName == key.FirstCurrencyISOName && x.Foreign.ISOName == key.SecondCurrencyISOName && x.IsActual);
                var rate2 = Context.Set<ExchangeRate>()
                    .OrderByDescending(x => x.TimestampUtc)
                    .FirstOrDefault(x => x.Base.ISOName == key.SecondCurrencyISOName && x.Foreign.ISOName == key.FirstCurrencyISOName && x.IsActual);
                var list = new List<ExchangeRate>();
                if (rate1 != null)
                {
                    list.Add(rate1);
                }
                if (rate2 != null)
                {
                    list.Add(rate2);
                }
                return list;
            });
        }

        public IList<ExchangeRate> GetAllActual(string baseCurrencyISOName)
        {
            return EnsureRepositoryException(() =>
            {
                var rates = Context.Set<ExchangeRate>()
                    .Include(x => x.Base)
                    .Include(x => x.Foreign)
                    .OrderByDescending(x => x.TimestampUtc)
                    .Where(x => x.IsActual && x.Base.ISOName == baseCurrencyISOName)
                    .ToList()
                    .DistinctBy(x => x.Foreign.ISOName)
                    .ToList();
                return rates;
            });
        }
    }
}
