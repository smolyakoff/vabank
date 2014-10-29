using System.Collections.Generic;
using System.Linq;
using Autofac;
using AutoMapper;

namespace VaBank.UI.Web.Modules
{
    public class ApplicationStartup : IStartable
    {
        private readonly ILifetimeScope _rootScope;

        public ApplicationStartup(ILifetimeScope rootScope)
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