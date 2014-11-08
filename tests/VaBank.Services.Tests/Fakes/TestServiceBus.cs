using System;
using VaBank.Common.Events;

namespace VaBank.Services.Tests.Fakes
{
    public static class TestServiceBus
    {
        private static volatile IServiceBus _instance;
        private static readonly object SyncRoot = new Object();

        public static IServiceBus Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                lock (SyncRoot)
                {
                    if (_instance == null)
                    {
                        _instance = new InMemoryServiceBus();
                    }
                }
                return _instance;
            }
        }
    }
}