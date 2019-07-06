// <copyright file="ServiceFabricUtilities.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.ServiceFabric
{
    using System;
    using System.Fabric;

    public static class ServiceFabricUtilities
    {
        /// <summary>
        /// Gets a value indicating whether we are running inside Service Fabric.
        /// We can know that if there's an env variable with the app name
        /// https://docs.microsoft.com/en-us/azure/service-fabric/service-fabric-environment-variables-reference
        /// </summary>
        /// <value>
        /// A value indicating wheter we are running inside Service Fabric.
        /// </value>
        public static bool IsRunningUnderServiceFabric =>
            Environment.GetEnvironmentVariable("Fabric_ApplicationName") != null;

        /// <summary>
        /// Gets build version of running service fabric app.
        /// During deployment we are updating the build version and can get it using FabricRuntime API
        /// (see deployment task documentation - https://docs.microsoft.com/en-us/azure/devops/pipelines/tasks/utility/service-fabric-versioning?view=azure-devops)
        /// </summary>
        /// <value>
        /// If running under service fabric app, returns app build version.
        /// otherwise, returns an empty string.
        /// </value>
        public static string AppBuildVersion => GetAppBuildVersion();

        private static string GetAppBuildVersion()
        {
            // If not running as service fabric app (it is possible to run as a plain web service), returns empty string
            if (!IsRunningUnderServiceFabric)
            {
                return string.Empty;
            }

            return FabricRuntime.GetActivationContext().GetServiceManifestVersion();
        }
    }
}