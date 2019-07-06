// <copyright file="ServiceFabricXmlConfigurationSource.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Configuration.ServiceFabric
{
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Making Service fabric configuration source that configuration builder can consume.
    /// </summary>
    public class ServiceFabricXmlConfigurationSource : FileConfigurationSource
    {
        /// <summary>
        /// Builds the <see cref="XmlConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="XmlConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            this.EnsureDefaults(builder);
            return new ServiceFabricXmlConfigurationProvider(this);
        }
    }
}