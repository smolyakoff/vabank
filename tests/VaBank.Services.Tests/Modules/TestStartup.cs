using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;

namespace VaBank.Services.Tests.Modules
{
    public class TestStartup : IStartable
    {
        private readonly ILifetimeScope _rootScope;

        public TestStartup(ILifetimeScope rootScope)
        {
            _rootScope = rootScope;
        }

        public void Start()
        {
            var automapperProfiles = _rootScope.Resolve<IEnumerable<Profile>>().ToList();
            automapperProfiles.ForEach(Mapper.AddProfile);
        }
    }
}
