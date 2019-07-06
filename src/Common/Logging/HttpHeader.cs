// <copyright file="HttpHeader.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    public static class HttpHeader
    {
        // For each operation we log the traffic source
        // i.e if the request triggered the operation is coming from test or runners
        public const string TestTrafficToken = "X-TestTraffic";

        public const string BearerToken = "Bearer";

        public const string CorrelationId = "x-correlation-id";

        // Use as a header in tests and runners for negative tests
        // to indicate we are expecting failures on purpose
        public const string ExpectingFail = "X-ExpectingFail";
    }
}