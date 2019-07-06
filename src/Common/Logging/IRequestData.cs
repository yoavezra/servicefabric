// <copyright file="IRequestData.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    public interface IRequestData
    {
        string CorrelationId { get; }

        string TenantId { get; }

        string TrafficSource { get; }
    }
}