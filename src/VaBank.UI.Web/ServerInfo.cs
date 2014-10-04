using System.Configuration;
using System.Web.Configuration;

namespace VaBank.UI.Web
{
    public sealed class ServerInfo
    {
        private readonly bool _isDebug;

        public ServerInfo()
        {
            var compilationSection = (CompilationSection) ConfigurationManager.GetSection("system.web/compilation");
            _isDebug = compilationSection.Debug;
        }

        public bool IsDebug
        {
            get { return _isDebug; }
        }
    }
}