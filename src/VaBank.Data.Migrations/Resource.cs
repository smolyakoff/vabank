using System.IO;
using System.Reflection;

namespace VaBank.Data.Migrations
{
    internal static class Resource
    {
        public static Stream OpenRead(string resourcePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullPath = string.Format("{0}.{1}", assembly.GetName().Name, resourcePath.Replace('/', '.'));
            var stream = assembly.GetManifestResourceStream(fullPath);
            return stream;
        }

        public static string ReadToEnd(string resourcePath)
        {
            using (var reader = new StreamReader(OpenRead(resourcePath)))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
