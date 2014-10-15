using System;
using System.Security.Claims;
using VaBank.Core.Common;
using VaBank.Core.Membership;

namespace VaBank.Core.App
{
    public class Operation : Entity, IDisposable
    {
        private bool _isDisposed;

        private Guid _id;

        private Guid? _userId;

        private string _name;

        private DateTime _timestampUtc;

        private string _clientId;


        public Operation(Guid id, DateTime timestampUtc, string name = null, ClaimsIdentity identity = null)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Operation id cannot be emtpy.");
            }
            Id = id;
            Name = string.IsNullOrEmpty("name") ? "APP-CHANGE" : name;
            TimestampUtc = timestampUtc;
            if (identity == null)
            {
                return;
            }
            var sid = identity.FindFirst(UserClaim.Types.UserId);
            UserId = sid == null ? null : (Guid?) Guid.Parse(sid.Value);
            var clientId = identity.FindFirst(UserClaim.Types.ClientId);
            ClientApplicationId = clientId == null ? null : clientId.Value;
        }

        protected Operation()
        {
        }

        public Guid Id
        {
            get { return GetIfNotDisposed(x => x._id); }
            set { _id = value; }
        }

        public Guid? UserId
        {
            get { return GetIfNotDisposed(x => x._userId); }
            private set { _userId = value; }
        }

        public DateTime TimestampUtc
        {
            get { return GetIfNotDisposed(x => x._timestampUtc); }
            private set { _timestampUtc = value; }
        }

        public string ClientApplicationId
        {
            get { return GetIfNotDisposed(x => x._clientId); }
            private set { _clientId = value; }
        }

        public string Name
        {
            get { return GetIfNotDisposed(x => x._name); }
            private set { _name = value; }
        }

        private T GetIfNotDisposed<T>(Func<Operation, T> getter)
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("Operation is already finished.");
            }
            return getter(this);
        }

        void IDisposable.Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
            }
        }
    }
}
