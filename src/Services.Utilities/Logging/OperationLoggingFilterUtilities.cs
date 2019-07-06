// <copyright file="OperationLoggingFilterUtilities.cs" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>

namespace ServiceSample.Services.Utilities.Logging
{
    using System;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Controllers;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;
    using ServiceSample.Common.Logging;
    using Microsoft.Extensions.Primitives;

    public static class OperationLoggingFilterUtilities
    {
        private const string OperationPropertyName = "HttpRequestExtensions-Operation";

        public static void AddOperation(ResourceExecutingContext context, string serviceComponent)
        {
            var actionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;

            var operationData = OperationData.CreateGeneric(serviceComponent, actionDescriptor.ActionName);

            Operation operation = Logger.StartOperation(operationData);
            operation.AddProperty("HttpRequestPort", context.HttpContext.Request.Host.Port ?? -1);
            operation.AddProperty("HttpRequestIsHttps", context.HttpContext.Request.IsHttps);

            // Set CorrelationId and traffic source at Task level such that it will be available to all downstream operations in the context of the request
            Logger.SetCorrelationId(GetCorrolationId(context.HttpContext.Request.Headers));

            if (context.HttpContext.Request.Headers.TryGetValue(
                HttpHeader.TestTrafficToken,
                out StringValues trafficSourceString))
            {
                Logger.SetTrafficSource(trafficSourceString);
            }

            // If we are expecting failure (in case of negative test), we won't log this operation as failed
            if (context.HttpContext.Request.Headers.ContainsKey(HttpHeader.ExpectingFail))
            {
                operation.AddProperty(HttpHeader.ExpectingFail, "true");
                operation.ExpectedToFail = true;
            }

            context.HttpContext.Items.Add(OperationPropertyName, operation);
        }

        private static string GetCorrolationId(IHeaderDictionary requestHeaders)
        {
            string correlationId;
            if (requestHeaders.TryGetValue(HttpHeader.CorrelationId, out StringValues clientCorrelationId))
            {
                correlationId = clientCorrelationId;
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
            }

            return correlationId;
        }

        public static void CloseOperation(ResourceExecutedContext context)
        {
            var operation = GetOperation(context);
            operation.Dispose();
        }

        public static void WrapOperationLogging(FilterContext context, Action<Operation> action)
        {
            Operation operation = GetOperation(context);
            try
            {
                action(operation);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
            {
                operation.LogFailure(
                    ErrorLevel.Error,
                    exception: ex,
                    errorMessage: "Error in telemetry code.");
            }
#pragma warning restore CA1031 // Do not catch general exception types
        }

        private static Operation GetOperation(ActionContext context) => (Operation)context.HttpContext.Items[OperationPropertyName];
    }
}