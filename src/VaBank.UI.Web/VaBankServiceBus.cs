using System;
using VaBank.Common.Events;

namespace VaBank.UI.Web
{
    public static class VaBankServiceBus
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