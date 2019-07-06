// <copyright file="CloudBornWebConfiguration.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Configuration
{
    using ServiceSample.Common.Logging;
    using ServiceSample.Services.Resources.Configuration;
    using ServiceSample.Services.Utilities.Authentication;

    public class CloudBornWebConfiguration
    {
        public CloudBornWebConfiguration(
            ServiceEnvironmentSettings serviceEnvironmentSettings,
            EnvironmentSettings environmentSettings,
            TokenValidationSettings tokenValidationSettings,
            AuthorizedResources authorizedResources)
        {
            this.ServiceEnvironmentSettings = serviceEnvironmentSettings;
            this.EnvironmentSettings = environmentSettings;
            this.TokenValidationSettings = tokenValidationSettings;
            this.AuthorizedResources = authorizedResources;
        }

        public ServiceEnvironmentSettings ServiceEnvironmentSettings { get; }

        public EnvironmentSettings EnvironmentSettings { get; }

        public TokenValidationSettings TokenValidationSettings { get; }

        public AuthorizedResources AuthorizedResources { get; }
    }
}
