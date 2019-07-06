// <copyright file="AuthorizedResources.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.CloudBornApplication.Service.CloudBornWeb.Configuration
{
    public class AuthorizedResources
    {
        // The Application that can publish ingest signals for testing purposes
        public string TestTrustedAppId { get; set; }
    }
}
