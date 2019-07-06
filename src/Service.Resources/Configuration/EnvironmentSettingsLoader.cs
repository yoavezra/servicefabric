// <copyright file="EnvironmentSettingsLoader.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Resources.Configuration
{
    public static class EnvironmentSettingsLoader
    {
        public static EnvironmentSettings Load(string resourceName)
        {
            return ResourceLoader.LoadConfigurations<EnvironmentSettings>(resourceName);
        }
    }
}