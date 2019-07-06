// <copyright file="OperationLogger.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Diagnostics.Tracing;
    using System.Threading.Tasks;

    /// <summary>
    /// Common logging class for trace events.
    /// Uses self-describing ETW events (ref: https://msdn.microsoft.com/en-us/library/dn904632(v=vs.85).aspx) instead of decorating the class
    /// with an EventSource attribute which will generate an ETW manifest at compile time.
    /// </summary>
    public sealed class OperationLogger : IOperationLogger, IDisposable
    {
        private readonly EventSource eventSource;
        private readonly string version;
        private readonly string service;
        private readonly IRequestData requestData;

        static OperationLogger()
        {
            // A workaround for the problem where ETW activities do not get tracked until Tasks infrastructure is initialized.
            // This problem will be fixed in .NET Framework 4.6.2.
            Task.Run(() => { });
        }

        public OperationLogger(
            string eventSourceName,
            string versionNumber,
            string service,
            IRequestData requestData)
        {
            this.eventSource = new EventSource(eventSourceName);
            this.version = versionNumber;
            this.service = service;
            this.requestData = requestData;
        }

        /// <summary>
        /// Method to log all operations
        /// </summary>
        /// <param name="operationData">Which operation is the error being raised from</param>
        /// <param name="dataCenter">Which dataCenter the current code runs on</param>
        /// <param name="operationResult">Defines what is the result of the operation</param>
        /// <param name="errorLevel">Error level - Critical, Error, Warning or None</param>
        /// <param name="startTime">Start time</param>
        /// <param name="propertyBag">A dictionary of values that provide context on the operation</param>
        /// <param name="message">A brief explanation of the result</param>
        /// <param name="errorSignature">A succinct error signature to identify that error and allow aggregation on that error type</param>
        /// <param name="durationMs">Time in milliseconds to complete operation</param>
        /// <param name="exception">Exception that contains exception stack trace and error message</param>
        /// <param name="httpStatusCode">Status code</param>
        /// <param name="tenantId">Tenant id</param>
        public void LogOperation(
            OperationData operationData,
            string dataCenter,
            OperationResult operationResult,
            ErrorLevel errorLevel,
            DateTime startTime,
            IDictionary<string, object> propertyBag,
            string message,
            string errorSignature,
            int durationMs,
            string exception = "",
            string httpStatusCode = "",
            string tenantId = "")
        {
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                tenantId = this.requestData.TenantId;
            }

            string correlationId = this.requestData.CorrelationId;
            string trafficSource = this.requestData.TrafficSource;

            if (this.eventSource.IsEnabled())
            {
                // The member names are used as Geneva event schema.
                // You can add more properties freely, but a name changing can cause Geneva operation logging to stop working.
                var data = new
                {
                    errorLevel = errorLevel.ToString(),
                    result = operationResult.ToString(),
                    service = this.service,
                    component = operationData.Component,
                    operationName = operationData.OperationName,
                    dataCenter = dataCenter,
                    durationMs = durationMs,
                    errorSignature = errorSignature,
                    message = message,
                    propertyBag = propertyBag.ToJsonString(),
                    tenantId = tenantId,
                    correlationId = correlationId,
                    startTime = startTime,
                    exception = exception,
                    version = this.version,
                    httpStatusCode = httpStatusCode,
                    trafficSource = trafficSource,
                };

                this.eventSource.Write(
                    EtwEventNames.TraceOperationEventName,
                    new EventSourceOptions { Level = EventLevel.Informational },
                    data);
            }

            Debug.WriteLine($"Operation: {operationData.OperationName}. Result: {operationResult}");
        }

        /// <summary>
        /// Method to log user operation
        /// </summary>
        /// <remarks>
        /// Following the Trace logging style of ETW eventing (self-describing events), this uses the EventSource.Write
        /// call to log an event with a self-describing schema that is inferred from the EventData sent in to the Write
        /// method. In the MA config of the listener agent, we use the unique event name to identify and listen to events
        /// logged by this method.
        /// </remarks>
        ///
        public void Dispose()
        {
            this.eventSource?.Dispose();
        }
    }
}