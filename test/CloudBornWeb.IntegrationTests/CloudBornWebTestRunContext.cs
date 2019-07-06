// <copyright file="CloudBornWebTestRunContext.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.CloudBornWeb.IntegrationTests
{
    using ServiceSample.CloudBornApplication.CloudBornWeb.ClientSdk;
    using ServiceSample.CloudBornApplication.Testing.Resources.Configuration;

    public class CloudBornWebTestRunContext
    {
        public ConnectionInfo ConnectionInfo { get; }

        // CloudBornWeb client for MS tenant user
        public CloudBornWebClient CloudBornWebClient { get; }

        public CloudBornWebClientBuilder CloudBornWebClientBuilder { get; }

        public CloudBornWebTestRunContext(ConnectionInfo connectionInfo, CloudBornWebClient client, CloudBornWebClientBuilder clientBuilder)
        {
            this.ConnectionInfo = connectionInfo;
            this.CloudBornWebClient = client;
            this.CloudBornWebClientBuilder = clientBuilder;
        }
    }
}
