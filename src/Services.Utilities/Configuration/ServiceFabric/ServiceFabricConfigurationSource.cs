// <copyright file="ServiceFabricConfigurationSource.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Configuration.ServiceFabric
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Making Service fabric configuration source that configuration builder can consume.
    /// </summary>
    public class ServiceFabricConfigurationSource : IConfigurationSource
    {
        public string PackageName { get; set; }

        public ServiceFabricConfigurationSource(string packageName)
        {
            this.PackageName = packageName;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new ServiceFabricConfigurationProvider(this.PackageName);
        }
    }
}