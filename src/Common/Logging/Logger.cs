// <copyright file="Logger.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is the static logging API for all services and components.
    /// It exposes Trace logger and Operation logger for logging traces and operations.
    /// It allows different trace-logging and operation-logging implementations to be placed globally for logging based on the context (i.e. service)
    /// </summary>
    /// <remarks>
    /// Usage:
    ///    Every service must initialize the Logging module by calling
    ///         Logger.Initialize(context)
    ///    with a suitable context value.
    ///    For example, for CloudBornWeb service, Service = CloudBornWebService
    ///
    /// By default, the TraceLogger and Operation logger get initialized to the no-op "Null" implementations.
    ///
    /// Code that needs to log traces should call
    ///         Logger.TraceLogger.XXX
    ///
    /// Operations should use the Operation class or call
    ///         Logger.OperationLogger.XXX
    /// </remarks>
    public static class Logger
    {
        /// <summary>
        /// Logger for Operations in the current service context
        /// </summary>
        /// <remarks>
        /// Default initialized to the OperationLoggerDefault (no-op implementation)
        /// </remarks>
        private static IOperationLogger operationLogger = new OperationLoggerDefault();

        private static RequestDataStore requestDataStore = new RequestDataStore();

        private static string dataCenter = string.Empty;

        public static void Initialize(string eventSourceName, string version, string service)
        {
            operationLogger = new OperationLogger(eventSourceName, version, service, requestDataStore);
        }

        public static Operation StartOperation(OperationData operationData)
        {
            var operation = new Operation(
                operationData,
                dataCenter,
                operationLogger);

            return operation;
        }

        public static void LogFailure(
            OperationData operationData,
            string message,
            ErrorLevel errorLevel,
            Exception exception = null,
            IDictionary<string, object> propertyBag = null)
        {
            operationLogger.LogOperation(
                operationData,
                dataCenter,
                OperationResult.Failure,
                errorLevel,
                DateTime.UtcNow,
                propertyBag,
                message,
                exception?.GetType().Name ?? string.Empty,
                0,
                exception?.ToString() ?? string.Empty,
                string.Empty,
                string.Empty);
        }

        public static void SetCorrelationId(string correlationId)
        {
            requestDataStore.SetCorrelationId(correlationId);
        }

        public static void SetTenantId(string tenantId)
        {
            requestDataStore.SetTenantId(tenantId);
        }

        public static void SetDataCenter(string dataCenterName)
        {
            dataCenter = dataCenterName;
        }

        public static void SetTrafficSource(string source)
        {
            requestDataStore.SetTrafficSource(source);
        }
    }
}
