using System;
using VaBank.Core.Common;
using VaBank.Core.Membership;

namespace VaBank.Core.App
{
    public class Operation : Entity, IDisposable
    {
        private bool _isDisposed;

        private Guid _id;

        private Guid? _userId;

        private User _user;

        private string _name;

        private DateTime _startedUtc;

        private DateTime? _finishedUtc;

        private ApplicationClient _client;

        private string _clientId;

        protected Operation()
        {
        }

        public Guid Id
        {
            get { return GetIfNotDisposed(x => x._id); }
            set { _id = value; }
        }

        public virtual User User
        {
            get { return GetIfNotDisposed(x => x._user); }
            private set
            {
                _user = value;
                _userId = value == null ? (Guid?)null : value.Id;
            }
        }

        public Guid? UserId
        {
            get { return GetIfNotDisposed(x => x._userId); }
            private set { _userId = value; }
        }

        public DateTime StartedUtc
        {
            get { return GetIfNotDisposed(x => x._startedUtc); }
            private set { _startedUtc = value; }
        }

        public DateTime? FinishedUtc
        {
            get { return GetIfNotDisposed(x => x._finishedUtc); }
            private set { _finishedUtc = value; }
        }

        public virtual ApplicationClient ApplicationClient
        {
            get { return GetIfNotDisposed(x => x._client); }
            private set
            {
                _client = value;
                _clientId = value == null ? null : value.Id;
            }
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
