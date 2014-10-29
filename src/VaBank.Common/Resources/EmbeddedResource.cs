using System;
using System.IO;
using System.Reflection;

namespace VaBank.Common.Resources
{
    public static class EmbeddedResource
    {
        public static Stream OpenRead(Assembly assembly, string resourcePath)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }
            if (string.IsNullOrEmpty(resourcePath))
            {
                throw new ArgumentNullException("resourcePath");
            }
            var fullPath = string.Format("{0}.{1}", assembly.GetName().Name, resourcePath.Replace('/', '.'));
            var stream = assembly.GetManifestResourceStream(fullPath);
            return stream;
        }

        public static string ReadAsString(Assembly assembly, string resourcePath)
        {
            var stream = OpenRead(assembly, resourcePath);
            if (stream == null)
            {
                return null;
            }
            using (var streamReader = new StreamReader(stream))
            {
                return streamReader.ReadToEnd();
            }
        }
    }
}
