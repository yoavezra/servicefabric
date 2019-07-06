// <copyright file="RequestDataStore.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    using System.Threading;

    /// <summary>
    /// Used to persist telemetry values across an asynchronous flow.
    /// We set the values at the beginning of the flow and this class make those values available to all the traces
    /// in the request asynchronous flow
    /// </summary>
    public class RequestDataStore : IRequestData
    {
        private static AsyncLocal<string> correlationId = new AsyncLocal<string>();

        private static AsyncLocal<string> tenantId = new AsyncLocal<string>();

        private static AsyncLocal<string> trafficSource = new AsyncLocal<string>();

        public string CorrelationId => correlationId.Value ?? string.Empty;

        public string TenantId => tenantId.Value ?? string.Empty;

        public string TrafficSource => trafficSource.Value ?? string.Empty;

#pragma warning disable CA1822 // Use static: AsyncLocal is static since it stores Task local state. It's exposed via instance to allow dependency injection
        public void SetCorrelationId(string id)
        {
            correlationId.Value = id;
        }

        public void SetTenantId(string id)
        {
            tenantId.Value = id;
        }

        public void SetTrafficSource(string source)
        {
            trafficSource.Value = source;
        }
#pragma warning disable CA1822
    }
}