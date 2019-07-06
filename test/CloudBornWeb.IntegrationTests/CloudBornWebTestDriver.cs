// <copyright file="CloudBornWebTestDriver.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.CloudBornWeb.IntegrationTests
{
    using System;
    using System.Threading.Tasks;
    using ServiceSample.CloudBornApplication.CloudBornWeb.ClientSdk;
    using ServiceSample.CloudBornApplication.Testing.Resources.Configuration;
    using ServiceSample.Common.ClientSdk;
    using ServiceSample.Common.IntegrationTests;

    public static class CloudBornWebTestDriver
    {
        private static CloudBornWebTestRunContext testRunContext;

        public static async Task<CloudBornWebTestRunContext> InitTestEnvironment(
            TestRunSettings testRunSettings,
            TrafficSource trafficSource)
        {
            // Init the environment only once in test run
            if (testRunContext != null)
            {
                return testRunContext;
            }

            var connectionInfo = ConnectionInfoReader.GetConnectionInfo(testRunSettings.Environment);

            var webClientBuilder = new CloudBornWebClientBuilder(connectionInfo, trafficSource);

            CloudBornWebClient webClient = webClientBuilder.Create();

            await HttpClientUtils.WaitForService(
                async () =>
                {
                    await webClient.GetHealth().ConfigureAwait(false);
                },
                connectionInfo.RetryOnFailedConnection).ConfigureAwait(false);

            testRunContext = new CloudBornWebTestRunContext(connectionInfo, webClient, webClientBuilder);

            return testRunContext;
        }
    }
}
