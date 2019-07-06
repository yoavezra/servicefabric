// <copyright file="CloudBornWorker.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWorker
{
    using System;
    using System.Collections.Generic;
    using System.Fabric;
    using System.Fabric.Description;
    using System.Globalization;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using ServiceSample.Services.Resources.Configuration;
    using Microsoft.ServiceFabric.Services.Communication.Runtime;
    using Microsoft.ServiceFabric.Services.Runtime;
    using ServiceSample.Common.Logging;

    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class CloudBornWorker : StatelessService
    {
        private readonly ICodePackageActivationContext activationContext;

        public CloudBornWorker(StatelessServiceContext context)
            : base(context)
        {
            this.activationContext = context.CodePackageActivationContext;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // Connection Cryptography Compliance: All connections must be utilizing TLS 1.2
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var configuration = CreateConfiguration(this.activationContext);

            Logger.SetDataCenter(configuration.DataCenter);

            await MonitorJob.Run(configuration, cancellationToken);
        }

        private static CloudBornWorkerConfiguration CreateConfiguration(ICodePackageActivationContext activationContext)
        {
            var configurationPackage = activationContext.GetConfigurationPackageObject("Config");

            ConfigurationSection serviceEnvironmentSection = configurationPackage.Settings.Sections["ServiceEnvironment"];
            string dataCenter = serviceEnvironmentSection.Parameters["DataCenterName"].Value;
            string environmentSettingsResourceName = serviceEnvironmentSection.Parameters["EnvironmentSettingsResourceName"].Value;
            EnvironmentSettings environmentSettings = EnvironmentSettingsLoader.Load(environmentSettingsResourceName);

            ConfigurationSection workerSection = configurationPackage.Settings.Sections["CloudBornWorker"];
            TimeSpan monitoringJobInterval = TimeSpan.FromMinutes(double.Parse(workerSection.Parameters["MonitoringJobIntervalInMinutes"].Value, CultureInfo.InvariantCulture));
            string externalPrincipals = environmentSettings.MonitoringAllowedExternalPrincipals;

            return new CloudBornWorkerConfiguration(
                dataCenter,
                monitoringJobInterval,
                externalPrincipals);
        }
    }
}
