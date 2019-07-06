// <copyright file="ServiceEnvironmentSettings.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    /// <summary>
    /// This class contains environment information for logging.
    /// </summary>
    public class ServiceEnvironmentSettings
    {
        /// <summary>
        /// This parameter contains the environment data center name for logging.
        /// </summary>
        public string DataCenterName { get; set; }

        /// <summary>
        /// Name of the environment resource file
        /// </summary>
        public string EnvironmentSettingsResourceName { get; set; }
    }
}