// <copyright file="ConnectionInfo.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Testing.Resources.Configuration
{
    using System;

    /// <summary>
    /// Holds connection details for a given environments.
    /// If a property value is not set in the configuration file, the property default value is used.
    /// </summary>
    public class ConnectionInfo
    {
#pragma warning disable CA1056 // Uri properties should not be strings
        public string ClusterUri { get; set; }
#pragma warning restore CA1056 // Uri properties should not be strings

        public int? WebPort { get; set; } = 8080;

        public int RetryOnFailedConnection { get; set; } = 1;
    }
}
