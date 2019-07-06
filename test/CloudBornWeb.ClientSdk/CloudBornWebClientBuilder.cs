// <copyright file="CloudBornWebClientBuilder.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.CloudBornWeb.ClientSdk
{
    using System;
    using ServiceSample.CloudBornApplication.Testing.Resources.Configuration;
    using ServiceSample.Common.ClientSdk;

    public class CloudBornWebClientBuilder
    {
        private readonly TrafficSource trafficSource;
        private readonly Uri serviceUri;

        public CloudBornWebClientBuilder(
            ConnectionInfo connectionInfo,
            TrafficSource trafficSource)
        {
            this.serviceUri = GetServiceUri(new Uri(connectionInfo.ClusterUri), connectionInfo.WebPort);
            this.trafficSource = trafficSource;
        }

        public CloudBornWebClient Create()
        {
            return new CloudBornWebClient(this.serviceUri, this.trafficSource);
        }

        private static Uri GetServiceUri(Uri clusterUri, int? port)
        {
            if (port.HasValue)
            {
                var uriBuilder = new UriBuilder(clusterUri)
                {
                    Port = port.Value,
                };
                return new Uri(uriBuilder.ToString());
            }

            return new Uri(clusterUri.AbsoluteUri);
        }
    }
}
