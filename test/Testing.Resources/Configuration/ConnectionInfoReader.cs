// <copyright file="ConnectionInfoReader.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Testing.Resources.Configuration
{
    using System.Reflection;
    using Newtonsoft.Json;
    using ServiceSample.Common.General;

    public static class ConnectionInfoReader
    {
        private const string DefaultEnvironment = "Local";
        private const string NS = "ServiceSample.CloudBornApplication.Testing.Resources";

        /// <summary>
        /// Used to get connection data for given environment
        /// </summary>
        /// <param name="environmentType">
        /// Type of the environment. Must have a matching json file under:
        /// \Resources\*environmentType*Connection.json
        /// If empty - use default environment (Local)
        /// </param>
        public static ConnectionInfo GetConnectionInfo(string environmentType = "")
        {
            if (string.IsNullOrWhiteSpace(environmentType))
            {
                environmentType = DefaultEnvironment;
            }

            string fileName = $"{environmentType}.Connection.json";

            var json = ResourceReader.GetResource(
                Assembly.GetExecutingAssembly(),
                NS,
                fileName);
            var connectionInfo = JsonConvert.DeserializeObject<ConnectionInfo>(json);

            return connectionInfo;
        }
    }
}
