// <copyright file="ServiceFabricConfigurationExtensions.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Configuration.ServiceFabric
{
    using Microsoft.Extensions.Configuration;

    public static class ServiceFabricConfigurationExtensions
    {
        /// <summary>
        /// Add <see cref="ServiceFabricConfigurationSource"/> (as another source of configurtion) to the configuration builder,
        /// that eventually will create <see cref="IConfiguration"/>.
        /// </summary>
        public static IConfigurationBuilder AddServiceFabricConfiguration(this IConfigurationBuilder builder, string packageName)
        {
            return builder.Add(new ServiceFabricConfigurationSource(packageName));
        }

        /// <summary>
        /// Add  <see cref="ServiceFabricConfigurationSource"/> (as another source of configurtion) to the configuration builder,
        /// that eventually will create <see cref="IConfiguration"/>
        /// This source is used to work in development environment when running a service as Web App.
        /// </summary>
        public static IConfigurationBuilder AddServiceFabricXmlConfiguration(this IConfigurationBuilder builder, string path)
        {
            var source = new ServiceFabricXmlConfigurationSource()
            {
                FileProvider = null,
                Path = path,
                ReloadOnChange = true,
            };

            source.ResolveFileProvider();
            return builder.Add(source);
        }
    }
}