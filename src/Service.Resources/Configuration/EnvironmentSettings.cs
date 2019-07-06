// <copyright file="EnvironmentSettings.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Resources.Configuration
{
    /// <summary>
    /// This class includes environment level (Int, PPE and Prod) configuration.
    /// The per-environment values are defined in the *environment*.Settings.json
    /// </summary>
    public class EnvironmentSettings
    {
        public string MonitoringAllowedExternalPrincipals { get; set; }
    }
}