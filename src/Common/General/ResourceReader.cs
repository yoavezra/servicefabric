// <copyright file="ResourceReader.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.General
{
    using System.IO;
    using System.Reflection;
    using System.Resources;

    public static class ResourceReader
    {
        public static string GetResource(Assembly executingAssembly, string namespaceName, string fileName)
        {
            var resourceName = $"{namespaceName}.{fileName}";
            using (Stream stream = executingAssembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    throw new MissingManifestResourceException($"Can't load {resourceName} from assembly {executingAssembly.FullName}");
                }

                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}