// <copyright file="IOperationLogger.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Common.Logging
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Interface for emitting structured Operation logs
    /// </summary>
    /// <remarks>
    /// Used by Operations log information about an operation (duration, success/failure
    /// result, and other diagnostics) to an event provider.
    ///
    /// This interface allows the specifics of the Operation logger/ETW provider implementation
    /// to be abstracted from the logic that uses it, making it possible to have custom
    /// implementations for logging operations based on the business context.
    /// </remarks>
    public interface IOperationLogger
    {
        void LogOperation(
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
            string tenantId = "");
    }
}