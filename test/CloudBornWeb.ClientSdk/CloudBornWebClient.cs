// <copyright file="CloudBornWebClient.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.CloudBornWeb.ClientSdk
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using ServiceSample.Common.ClientSdk;
    using ServiceSample.Common.Logging;

    public sealed class CloudBornWebClient : IDisposable
    {
        private const string RequestPrefix = "api";

        private readonly Uri baseAddress;
        private readonly HttpClient httpClient;

        static CloudBornWebClient()
        {
            // skip certificate validation in test client
#pragma warning disable CA5359 // Do Not Disable Certificate Validation
            ServicePointManager.ServerCertificateValidationCallback =
                (sender, certificate, chain, errors) => true;
#pragma warning restore CA5359 // Do Not Disable Certificate Validation
        }

        public CloudBornWebClient(Uri baseAddress, TrafficSource trafficSource)
        {
            this.baseAddress = baseAddress;
            this.httpClient = new HttpClient();
            this.httpClient.DefaultRequestHeaders.Add(HttpHeader.TestTrafficToken, trafficSource.ToString());
        }

        public async Task<HttpResponseMessage> GetHealth()
        {
            var uri = this.GetFullUri("Diagnostics/Health");

            var response = await HttpClientUtils.GetResponseAsync(this.httpClient, uri).ConfigureAwait(false);

            return response;
        }

        private Uri GetFullUri(string relativePath)
        {
            var uri = new Uri(this.baseAddress, $"{RequestPrefix}/{relativePath}");
            return uri;
        }

        public void Dispose()
        {
            this.httpClient.Dispose();
        }
    }
}
