using System;
using System.Data;
using System.Data.Entity;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.Owin;
using VaBank.Services.Contracts.Common;

namespace VaBank.UI.Web.Api.Infrastructure.Filters
{
    public class TransactionAttribute : ActionFilterAttribute
    {
        private DbContextTransaction _transaction;

        public TransactionAttribute()
        {
            IsolationLevel = IsolationLevel.ReadCommitted;
        }

        public IsolationLevel IsolationLevel { get; set; }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var lifetimeScope = actionContext.Request.GetOwinContext().GetAutofacLifetimeScope();
            var dbContext = lifetimeScope.Resolve<DbContext>();
            _transaction = dbContext.Database.BeginTransaction(IsolationLevel);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            try
            {
                if (actionExecutedContext.Exception == null)
                {
                    _transaction.Commit();
                }
                else
                {
                    OnException((dynamic)actionExecutedContext.Exception);
                }
            }
            finally
            {
                _transaction.Dispose();
                _transaction = null;
            }
        }

        private void OnException(ServiceException exception)
        {
            if (exception.TransactionRollback)
            {
                _transaction.Rollback();
            }
            else
            {
                _transaction.Commit();
            }
        }

        private void OnException(Exception exception)
        {
            _transaction.Rollback();
        }
    }
}