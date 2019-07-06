// <copyright file="OperationLoggerDefault.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    public class OperationLoggerDefault : IOperationLogger
    {
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
            Debug.WriteLine($"Operation: {operationData.OperationName}. Component: {operationData.Component}. Result: {operationResult}");
        }
    }
}