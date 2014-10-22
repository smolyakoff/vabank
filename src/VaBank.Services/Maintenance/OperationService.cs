using System;
using VaBank.Core.App;
using VaBank.Services.Common;
using VaBank.Services.Contracts.Common;
using VaBank.Services.Contracts.Maintenance;

namespace VaBank.Services.Maintenance
{
    public class OperationService : IOperationService
    {
        private readonly ServiceOperationProvider _provider;

        private readonly IOperationRepository _repository;

        public OperationService(OperationServiceDependencies dependencies)
        {
            dependencies.EnsureIsResolved();
            _provider = dependencies.Provider;
            _repository = dependencies.Repository;
        }

        public bool HasCurrent
        {
            get { return _provider.HasCurrent; }
        }

        public Guid Current
        {
            get { return _provider.GetCurrent().Id; }
        }

        public void Stop(Guid operationId)
        {
            try
            {
                var operation = _repository.Find(operationId);
                _repository.Stop(operation);
            }
            catch (Exception ex)
            {
                throw new ServiceException("Can't stop operation", ex);
            }
        }
    }
}
