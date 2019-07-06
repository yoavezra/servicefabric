// <copyright file="CloudBornWorkerConfiguration.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWorker
{
    using System;

    public class CloudBornWorkerConfiguration
    {
        public TimeSpan MonitoringJobInterval { get; }

        public string ExternalPrincipals { get; }

        public string DataCenter { get; }

        public CloudBornWorkerConfiguration(
            string dataCenter,
            TimeSpan monitoringJobInterval,
            string externalPrincipals)
        {
            this.MonitoringJobInterval = monitoringJobInterval;
            this.ExternalPrincipals = externalPrincipals;
            this.DataCenter = dataCenter;
        }
    }
}
